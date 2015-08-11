using Axel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    public class LiftSession : IComparable
    {
        private Stack<LiftSet> _liftSets;
        private List<WeightStack> _sessionWeightStacks;
        private List<WeightStack> _pulledWeightStacks;

        private int _weightSetMoves;
        private int _usedPlatesCount;
        private int _currentTargetIndex;
        private decimal _currentTargetWeight;
        private decimal _barWeight;
        private decimal[] _targets;

        #region Constants

        private const int MaximumFinalIndexDelta = 49;
        private const int MaximumWeightDelta = 699;
        private const int MaximumMoves = 99;
        private const int MaximumPlateSets = 49;
        private const int MaximumFN = 178505050;

        #endregion

        #region Properties

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

        public LiftSession()
        {
            _liftSets = new Stack<LiftSet>();
            _pulledWeightStacks = new List<WeightStack>();
            _weightSetMoves = 0;
            _barWeight = 0;
            _usedPlatesCount = 0;
            _currentTargetIndex = -1;
        }

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

        public void CreateBaseSet()
        {
            LiftSet baseLiftSet = new LiftSet();
            baseLiftSet.Bar.BarWeight = _barWeight;
            baseLiftSet.Bar.TotalWeight = baseLiftSet.Bar.BarWeight;
            LiftSets.Push(baseLiftSet);
        }

        #region Adding Plates

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

        public bool CanAddPlates()
        {
            if (_sessionWeightStacks.Any(ws => ws.Plates.Count > 0))
                return true;

            return false;
        }

        public bool CanAddThesePlates(PlateSet neededPlates)
        {
            if (PulledPlatesAvailable(neededPlates) || SessionPlatesAvailable(neededPlates))
                return true;

            return false;
        }

        private PlateSet GetPlateSetFromPulledStack(PlateSet neededPlateSet)
        {
            PlateSet pulledPlateSet = new PlateSet(_pulledWeightStacks.FirstOrDefault(s => s.Weight == neededPlateSet.Weight).Plates.Pop());
            return pulledPlateSet;
        }

        private PlateSet GetPlateSetFromSessionStack(PlateSet neededPlateSet)
        {
            PlateSet sessionPlateSet = new PlateSet(_sessionWeightStacks.FirstOrDefault(s => s.Weight == neededPlateSet.Weight).Plates.Pop());
            return sessionPlateSet;
        }

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

        #endregion

        #region Stripping Plates

        public bool CanRemovePlates()
        {
            if (_liftSets != null
                && _liftSets.Count > 0
                && _liftSets.Peek().Bar.LoadedPlates.Count > 0)
                return true;

            return false;
        }

        public void StripPlates(int setsToRemove)
        {
            if (setsToRemove > 0)
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

        #endregion

        internal void UpdateTargetIndex(decimal[] targetSets)
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

        internal bool AtFinalSet(decimal[] targetSets)
        {
            LiftSet currentLiftSet = new LiftSet(this.LiftSets.Peek());

            if (currentLiftSet != null
                && CurrentTargetIndex == targetSets.Count() - 1
                && currentLiftSet.Bar.TotalWeight == targetSets[CurrentTargetIndex])
                return true;
            return false;
        }

        internal decimal TargetDifference()
        {
            decimal targetWeight = this.CurrentTargetWeight;
            decimal currentWeight = this._liftSets.Peek().Bar.TotalWeight;
            decimal weightDifference = Math.Abs(targetWeight - currentWeight);
            return weightDifference;
        }

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

        public int DistanceToFinalIndex()
        {
            int delta = (_targets.Count() - 1) - CurrentTargetIndex;
            return delta;
        }

        /// <summary>
        /// Cost of initial node to n (current node)
        /// </summary>
        /// <returns>decimal</returns>
        public decimal gn()
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
        public decimal hn()
        {
            //hn = a(max(b) + 1)(max(c) + 1)(max(d) + 1) + b(max(c) + 1)(max(d) + 1)
            decimal cost = (MaximumFinalIndexDelta * (MaximumWeightDelta + 1) * (MaximumPlateSets + 1) * (MaximumMoves + 1))
                        + MaximumWeightDelta * (MaximumPlateSets + 1) * (MaximumMoves + 1);

            decimal targetWeightDifference = TargetDifference();
            decimal distanceToTargetIndex = DistanceToFinalIndex();

            cost = (distanceToTargetIndex * (MaximumWeightDelta + 1) * (MaximumPlateSets + 1) * (MaximumMoves + 1))
                        + targetWeightDifference * (MaximumPlateSets + 1) * (MaximumMoves + 1);

            return cost;
        }

        /// <summary>
        /// Returns fn to determine best node.
        /// f(n) = h(n) + g(n) OR
        /// f(n) = a(max(b) + 1)(max(c) + 1)(max(d) + 1) + b(max(c) + 1)(max(d) + 1) + c(max(d) + 1) + d
        /// </summary>
        /// <returns>decimal</returns>
        public decimal fn()
        {
            //f(n) = a(max(b) + 1)(max(c) + 1)(max(d) + 1) + b(max(c) + 1)(max(d) + 1) + c(max(d) + 1) + d
            decimal val = hn() + gn();
            return val;
        }

        #endregion

        #region Comparable

        public int CompareTo(object obj)
        {
            if (obj == null)
                return -1;

            LiftSession session = obj as LiftSession;

            decimal thisFn = fn();
            decimal objFn = session.fn();

            if (thisFn > objFn)
                return -1;
            else if (thisFn < objFn)
                return 1;

            return -1;
        }

        #endregion


    }
}
