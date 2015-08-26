using Axel.Data.Search;
using Axel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    //TODO: Revisit Expand() method for catching when we have gone past the target weight (which causes search to spin out when an impossible target weight is entered)
    /// <summary>
    /// Class that gets expanded for searching. Implements IBestFirstSearchable and IComparable
    /// </summary>
    [Serializable]
    public class LiftSession : IBestFirstSearchable<LiftSession>
    {
        #region Members

        private Stack<LiftSet> _liftSets;
        private List<WeightStack> _sessionWeightStacks;
        private List<WeightStack> _pulledWeightStacks;
        private int _weightSetMoves;
        private int _usedPlatesCount;
        private int _currentTargetIndex;
        private decimal _currentTargetWeight;
        private decimal _barWeight;
        private decimal[] _targets;

        #endregion

        #region Constants

        /// <summary>
        /// Used for creating unique scaling value (obsolete)
        /// </summary>
        private const int MaximumFinalIndexDelta = 49;
        private const int MaximumWeightDelta = 699;
        private const int MaximumMoves = 99;
        private const int MaximumPlateSets = 49;
        private const int MaximumFN = 178505050;

        #endregion

        #region Properties

        /// <summary>
        /// Target weights to hit.
        /// </summary>
        public decimal[] Targets
        {
            get
            {
                return _targets;
            }
            set
            {
                _targets = value;
            }
        }

        /// <summary>
        /// Current weight being searched for
        /// </summary>
        public decimal CurrentTargetWeight
        {
            get
            {
                return _currentTargetWeight;
            }
            set
            {
                _currentTargetWeight = value;
            }
        }

        /// <summary>
        /// Current target index being searched for
        /// </summary>
        public int CurrentTargetIndex
        {
            get
            {
                return _currentTargetIndex;
            }
            set
            {
                _currentTargetIndex = value;
            }
        }

        /// <summary>
        /// Total number of plates pulled from session stack
        /// </summary>
        public int UsedPlatesCount
        {
            get
            {
                return _usedPlatesCount;
            }
            set
            {
                _usedPlatesCount = value;
            }
        }

        /// <summary>
        /// Stack of weights that can be pulled from to load the bar
        /// </summary>
        public List<WeightStack> PulledWeightStacks
        {
            get
            {
                return _pulledWeightStacks;
            }
            set
            {
                _pulledWeightStacks = value;
            }
        }

        /// <summary>
        /// Total available weights. Pulled fro to load Pulled weight stack
        /// </summary>
        public List<WeightStack> SessionWeightStacks
        {
            get
            {
                return _sessionWeightStacks;
            }
            set
            {
                _sessionWeightStacks = value;
            }
        }

        /// <summary>
        /// Weight of bar
        /// </summary>
        public decimal BarWeight
        {
            get
            {
                return _barWeight;
            }
            set
            {
                _barWeight = value;
            }
        }

        /// <summary>
        /// Stack that tracks the lifting sets we have passed
        /// </summary>
        public Stack<LiftSet> LiftSets
        {
            get
            {
                return _liftSets;
            }
            set
            {
                _liftSets = value;
            }
        }

        /// <summary>
        /// Number of times weight has been loaded or unloaded
        /// </summary>
        public int WeightSetMoves
        {
            get
            {
                return _weightSetMoves;
            }
            set
            {
                _weightSetMoves = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs default LiftSession
        /// </summary>
        public LiftSession()
        {
            _liftSets = new Stack<LiftSet>();
            _pulledWeightStacks = new List<WeightStack>();
            _sessionWeightStacks = new List<WeightStack>();
            _weightSetMoves = 0;
            _barWeight = 0;
            _usedPlatesCount = 0;
            _currentTargetIndex = -1;
            _currentTargetWeight = -1;
            _targets = new decimal[0];
        }

        /// <summary>
        /// Constructs default lift session and first default liftset
        /// </summary>
        /// <param name="initialize">true to initialize</param>
        public LiftSession(bool initialize)
        {
            if (initialize)
            {
                CreateBaseSet();
            }

            _liftSets = new Stack<LiftSet>();
            _pulledWeightStacks = new List<WeightStack>();
            _sessionWeightStacks = new List<WeightStack>();
            _weightSetMoves = 0;
            _barWeight = 0;
            _usedPlatesCount = 0;
            _currentTargetIndex = -1;
            _currentTargetWeight = -1;
            _targets = new decimal[0];
        }

        /// <summary>
        /// Constructs LiftSession from existing LiftSession
        /// </summary>
        /// <param name="session">Existing LiftSession</param>
        public LiftSession(LiftSession session)
        {
            _liftSets = Cloner.Clone(session.LiftSets);
            _barWeight = session.BarWeight;
            _currentTargetIndex = session.CurrentTargetIndex;
            _currentTargetWeight = session.CurrentTargetWeight;
            _pulledWeightStacks = Cloner.Clone(session.PulledWeightStacks);
            _sessionWeightStacks = Cloner.Clone(session.SessionWeightStacks);
            _usedPlatesCount = session.UsedPlatesCount;
            _weightSetMoves = session.WeightSetMoves;
            _targets = session.Targets;
        }

        #endregion

        #region Methods

        //TODO: Remove this and add it to constructor
        /// <summary>
        /// Creates initial base LiftSet and adds it to session
        /// </summary>
        public void CreateBaseSet()
        {
            LiftSet baseLiftSet = new LiftSet();
            baseLiftSet.Bar.BarWeight = _barWeight;
            baseLiftSet.Bar.TotalWeight = baseLiftSet.Bar.BarWeight;
            LiftSets.Push(baseLiftSet);
        }

        /// <summary>
        /// Adds PlateSet to top LiftSet
        /// </summary>
        /// <param name="plateSet">PlateSet to add</param>
        public void AddPlates(PlateSet plateSet)
        {
            if(!PulledPlatesAvailable(plateSet))
            {
                if (SessionPlatesAvailable(plateSet))
                    AddToPulledPlateStack(plateSet);
            }

            if (PulledPlatesAvailable(plateSet))
            {
                PlateSet newPlateSet = GetPlateSetFromPulledStack(plateSet);

                if(_liftSets != null && _liftSets.Count > 0)
                {
                    LiftSet newLiftSet = new LiftSet(_liftSets.Peek());
                    newLiftSet.AddPlateSetToBar(plateSet);
                    _liftSets.Push(newLiftSet);
                    _weightSetMoves++;
                }
            }
        }

        /// <summary>
        /// Determines if plates can be added from session weight stacks
        /// </summary>
        /// <returns>true if plates can be added</returns>
        public bool CanAddPlates()
        {
            if (_sessionWeightStacks.Any(ws => ws.Plates.Count > 0))
                return true;

            return false;
        }

        /// <summary>
        /// Determines if a provided PlateSet can be added
        /// </summary>
        /// <param name="neededPlates">true if session PlateSet is available for use</param>
        /// <returns></returns>
        public bool CanAddThesePlates(PlateSet neededPlates)
        {
            if (PulledPlatesAvailable(neededPlates) || SessionPlatesAvailable(neededPlates))
                return true;

            return false;
        }

        /// <summary>
        /// Pulls PlateSet from Session's PulledStack
        /// </summary>
        /// <param name="neededPlateSet">PlateSet to pull</param>
        /// <returns></returns>
        private PlateSet GetPlateSetFromPulledStack(PlateSet neededPlateSet)
        {
            PlateSet pulledPlateSet = new PlateSet(_pulledWeightStacks.FirstOrDefault(s => s.Weight == neededPlateSet.Weight).Plates.Pop());
            return pulledPlateSet;
        }

        /// <summary>
        /// Pulls PlateSet from Session's SessionStack
        /// </summary>
        /// <param name="neededPlateSet">PlateSet to pull</param>
        /// <returns></returns>
        private PlateSet GetPlateSetFromSessionStack(PlateSet neededPlateSet)
        {
            PlateSet sessionPlateSet = new PlateSet(_sessionWeightStacks.FirstOrDefault(s => s.Weight == neededPlateSet.Weight).Plates.Pop());
            return sessionPlateSet;
        }

        /// <summary>
        /// Adds PlateSet to Session's PulledStack
        /// </summary>
        /// <param name="plateSet"></param>
        private void AddToPulledPlateStack(PlateSet plateSet)
        {
            WeightStack stack = _pulledWeightStacks.FirstOrDefault(s => s.Weight == plateSet.Weight);

            if (stack == null)
            {
                WeightStack neededStack = new WeightStack(plateSet.Weight, 1);
                _pulledWeightStacks.Add(new WeightStack(neededStack));
            }

            _pulledWeightStacks.FirstOrDefault(s => s.Weight == plateSet.Weight).Plates.Push(GetPlateSetFromSessionStack(plateSet));
            _usedPlatesCount = _usedPlatesCount + 2;
        }

        /// <summary>
        /// Determines if Session's PulledStack has a PlateSet available for use
        /// </summary>
        /// <param name="plateSet">PlateSet to Pull</param>
        /// <returns>true if PlateSet is available in PulledStack</returns>
        private bool PulledPlatesAvailable(PlateSet plateSet)
        {
            if (_pulledWeightStacks != null && _pulledWeightStacks.Count > 0)
            {
                WeightStack stack = _pulledWeightStacks.FirstOrDefault(s => s.Weight == plateSet.Weight);

                if (stack != null)
                {
                    WeightStack pulledStack = new WeightStack(stack);

                    if (pulledStack != null && pulledStack.Plates.Count > 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if Session's SessionStack has a PlateSet available for use
        /// </summary>
        /// <param name="plateSet">PlateSet to pull</param>
        /// <returns>true if PlateSet is available</returns>
        private bool SessionPlatesAvailable(PlateSet plateSet)
        {
            if (_sessionWeightStacks != null && _sessionWeightStacks.Count > 0)
            {
                WeightStack stack = _sessionWeightStacks.FirstOrDefault(s => s.Weight == plateSet.Weight);

                if (stack != null)
                {
                    WeightStack sessionStack = new WeightStack(stack);

                    if (sessionStack != null && sessionStack.Plates.Count > 0)
                        return true;
                }
            }
            return false;
        }

        
        /// <summary>
        /// Determines if plates can be removed
        /// </summary>
        /// <returns>true if plates can be removed safely</returns>
        public bool CanRemovePlates()
        {
            if (_liftSets != null
                && _liftSets.Count > 0
                && _liftSets.Peek().Bar.LoadedPlates.Count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Strips plates from bar and adds to move count
        /// </summary>
        /// <param name="setsToRemove">PlateSets to remove</param>
        public void StripPlates(int setsToRemove)
        {
            if (setsToRemove > 0 && _liftSets.Count > 0)
            {
                LiftSet newSet = new LiftSet(_liftSets.Peek());
                List<PlateSet> removedPlateSets = new List<PlateSet>();

                removedPlateSets = newSet.Bar.RemovePlates(setsToRemove);

                _liftSets.Push(newSet);
                _weightSetMoves++;

                if (removedPlateSets != null && removedPlateSets.Count > 0)
                {
                    foreach (PlateSet removedPlateSet in removedPlateSets)
                    {
                        WeightStack stackAddingTo = _pulledWeightStacks.FirstOrDefault(p => p.Weight == removedPlateSet.Weight);

                        if (stackAddingTo != null)
                        {
                            stackAddingTo.Plates.Push(removedPlateSet);
                        }
                    }
                }
            }
        }

        //TODO: Remove this and add logic to add & remove plates (maybe expand?)
        /// <summary>
        /// Updates the current target being searched for
        /// </summary>
        /// <param name="targetSets"></param>
        public void UpdateTargetIndex(decimal[] targetSets)
        {
            LiftSet currentLiftSet = new LiftSet(this.LiftSets.Peek());
            if (currentLiftSet != null && CurrentTargetIndex < targetSets.Length - 1)
            {
                if (currentLiftSet.Bar.TotalWeight == targetSets[CurrentTargetIndex])
                {
                    //if not on the final target, increment it. Otherwise, leave it
                    if (CurrentTargetIndex != targetSets.Length - 1)
                    {
                        CurrentTargetIndex = CurrentTargetIndex + 1;
                        CurrentTargetWeight = targetSets[CurrentTargetIndex];
                    }
                }
            }
        }

        /// <summary>
        /// Weight difference between where current LiftSet is and where it needs to be
        /// </summary>
        /// <returns></returns>
        internal decimal TargetDifference()
        {
            decimal targetWeight = this.CurrentTargetWeight;
            decimal currentWeight = this._liftSets.Peek().Bar.TotalWeight;
            decimal weightDifference = targetWeight - currentWeight;

            if (weightDifference > 0)
                return weightDifference;
            else
                return 0;
        }

        /// <summary>
        /// Calculates total number of plates pulled so far
        /// </summary>
        /// <returns>Number of plates used (loaded & not loaded)</returns>
        internal int PlateSetsUsed()
        {
            int plateSetsUsed = 0;

            LiftSet currentLiftSet = LiftSets.Peek();

            if (currentLiftSet != null)
            {
                plateSetsUsed = plateSetsUsed + currentLiftSet.Bar.LoadedPlates.Count;
            }

            if(PulledWeightStacks != null && PulledWeightStacks.Count > 0)
            {
                foreach(WeightStack stack in PulledWeightStacks)
                {
                    plateSetsUsed = plateSetsUsed + stack.Plates.Count;
                }
            }

            return plateSetsUsed;
        }

        /// <summary>
        /// Number of LiftSets away from solution
        /// </summary>
        /// <returns>Set away from solution</returns>
        internal int DistanceToFinalIndex()
        {
            int delta = (_targets.Count() - 1) - CurrentTargetIndex;
            return delta;
        }

        #endregion

        #region Interface Members

        /// <summary>
        /// Cost of initial node to n (current node). Number of plate sets used.
        /// </summary>
        /// <returns>decimal</returns>
        public decimal Gn()
        {
            return PlateSetsUsed();
        }

        /// <summary>
        /// Cost of getting from n to final node. Approximate number of plate sets to still be used
        /// </summary>
        /// <returns>decimal</returns>
        public decimal Hn()
        {
            //initialize value very high to avoid making session appear at top of heap
            decimal approxPlateSetsToUse = 1000;

            //set the approximate weight to add to the difference to the next target
            decimal approxTotalWeightToAdd = TargetDifference();

            //only want to compute deltas between targets if we are more than 1 away from the end
            if (CurrentTargetIndex < Targets.Count() - 1)
            {
                //for each target weight, starting with the current compute delta
                for (int i = CurrentTargetIndex; i < Targets.Count() - 1; i++)
                {
                    decimal currentTargetWeight = Targets[i];
                    decimal nextTargetWeight = Targets[i + 1];

                    //add delta to the approximate total weight to add
                    approxTotalWeightToAdd = approxTotalWeightToAdd + Math.Abs(currentTargetWeight - nextTargetWeight);
                }
            }

            //get the largest plateset at our disposal
            WeightStack lightestWeightStack = SessionWeightStacks.OrderByDescending(p => p.Weight).LastOrDefault();

            if (lightestWeightStack != null)
            {
                //double the weight of that plate so we can get the weight of a plateset at that weight
                decimal lightestPlateSetWeight = 2 * lightestWeightStack.Weight;

                //divide the approximate total by the plateset weight for the estimated number of plates to be used
                approxPlateSetsToUse = approxTotalWeightToAdd / lightestPlateSetWeight;
            }

            return approxPlateSetsToUse;
        }

        /// <summary>
        /// Cost of initial node to n (current node)
        /// </summary>
        /// <returns>decimal</returns>
        public decimal GnByPlatesAndMoves()
        {
            //gn = c(max(d) + 1) + d
            decimal cost = (MaximumPlateSets * (MaximumMoves + 1)) + MaximumMoves;

            cost = (PlateSetsUsed() * (WeightSetMoves + 1)) + WeightSetMoves;

            return cost;
        }

        /// <summary>
        /// Cost of getting from n to final node
        /// </summary>
        /// <returns>decimal</returns>
        public decimal HnByWeightIndexDifference()
        {
            //hn = a(max(b) + 1)(max(c) + 1)(max(d) + 1) + b(max(c) + 1)(max(d) + 1)
            decimal cost = (MaximumFinalIndexDelta * (MaximumWeightDelta + 1) * (MaximumPlateSets + 1) * (MaximumMoves + 1))
                        + MaximumWeightDelta * (MaximumPlateSets + 1) * (MaximumMoves + 1);

        decimal targetWeightDifference = TargetDifference();
        decimal distanceToTargetIndex = DistanceToFinalIndex();

        cost = (distanceToTargetIndex* (MaximumWeightDelta + 1) * (MaximumPlateSets + 1) * (MaximumMoves + 1))
                        + targetWeightDifference* (MaximumPlateSets + 1) * (MaximumMoves + 1);

            return cost;
        }

    /// <summary>
    /// Returns fn to determine best node.
    /// f(n) = h(n) + g(n) OR
    /// f(n) = a(max(b) + 1)(max(c) + 1)(max(d) + 1) + b(max(c) + 1)(max(d) + 1) + c(max(d) + 1) + d
    /// </summary>
    /// <returns>decimal</returns>
    public decimal Fn()
        {
            //f(n) = a(max(b) + 1)(max(c) + 1)(max(d) + 1) + b(max(c) + 1)(max(d) + 1) + c(max(d) + 1) + d
            decimal val = Hn() + Gn();
            return val;
        }

        /// <summary>
        /// Expands session for an array next possible lift sessions
        /// </summary>
        /// <returns>ListSession[]</returns>
        public LiftSession[] Expand()
        {
            //Create array for storing expandedSessions
            List<LiftSession> expandedSessions = new List<LiftSession>();

            //if plates are available for adding
            if (CanAddPlates())
            {
                //for each stack of weights available for pulling
                foreach (WeightStack stack in SessionWeightStacks)
                {
                    //Create and initialize a plateset from this weight stack
                    PlateSet plateSetToAdd = new PlateSet();
                    plateSetToAdd.InitializePlates(stack.Weight);

                    //if we can add these plates, add them to the new session
                    if (CanAddThesePlates(plateSetToAdd))
                    {
                        //create a new session with these plates
                        LiftSession newSession = new LiftSession(this);
                        newSession.AddPlates(plateSetToAdd);

                        //update target index that have likely changed
                        newSession.UpdateTargetIndex(_targets);

                        //add this session into the list
                        expandedSessions.Add(newSession);
                    }
                }
            }

            //if plates are capable of being removed
            if (CanRemovePlates())
            {
                //if we have a liftset available for removing from
                if (LiftSets.Count > 0)
                {
                    //get the top/current lift set for the session
                    LiftSet currentLiftSet = new LiftSet(LiftSets.Peek());

                    //if the lift set is valid and there are plates to remove from it
                    if (currentLiftSet != null && currentLiftSet.CanRemovePlates())
                    {
                        //see how many sets of plates are on the bar
                        int plateSetCount = LiftSets.Peek().Bar.LoadedPlates.Count;

                        //for each plateset on the bar, create a new session and strip
                        for (int i = 1; i <= plateSetCount; i++)
                        {
                            //create the session and strip the plates (counts as 1 move)
                            LiftSession newSession = new LiftSession(this);
                            newSession.StripPlates(i);

                            //update target index that has likely changed
                            newSession.UpdateTargetIndex(_targets);

                            //add the new session to the list
                            expandedSessions.Add(newSession);
                        }
                    }
                }
            }

            //return the expanded array
            return expandedSessions.ToArray();
        }

        /// <summary>
        /// Determines if two LiftSession are the same.
        /// </summary>
        /// <param name="compareItem">LiftSession</param>
        /// <returns></returns>
        public bool IsEquivalentNode(LiftSession compareItem)
        {
            if (CurrentTargetIndex == compareItem.CurrentTargetIndex
                && LiftSets.Peek().Bar.TotalWeight == compareItem.LiftSets.Peek().Bar.TotalWeight)
                return true;

            return false;
        }

        /// <summary>
        /// Compares LiftSessions
        /// </summary>
        /// <param name="obj">LiftSession being compared</param>
        /// <returns>result of compare</returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return -1;

            LiftSession session = obj as LiftSession;

            decimal thisFn = Fn();
            decimal objFn = session.Fn();

            if (thisFn > objFn)
                return -1;
            else if (thisFn < objFn)
                return 1;

            return -1;
        }

        #endregion
    }
}
