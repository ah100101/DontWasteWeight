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
        
        #region Properties

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

        public List<WeightStack> PulledWeightStack
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
            //this is copying the bar by reference, how am i supposed to do this????
            //_liftSets = new Stack<LiftSet>(session.LiftSets);
            _liftSets = Cloner.Clone(session.LiftSets);
            //_liftSets = new Stack<LiftSet>(new Stack<LiftSet>(session.LiftSets));
            //var clonedStack = new Stack<T>(new Stack<T>(oldStack));
            _barWeight = session.BarWeight;
            _currentTargetIndex = session.CurrentTargetIndex;
            _currentTargetWeight = session.CurrentTargetWeight;
            _pulledWeightStacks = new List<WeightStack>(session.PulledWeightStack);
            _sessionWeightStacks = new List<WeightStack>(session.SessionWeightStacks);
            _usedPlatesCount = session.UsedPlatesCount;
            _weightSetMoves = session.WeightSetMoves;
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
                newSet.Bar.RemovePlates(setsToRemove);
                _liftSets.Push(newSet);
                _weightSetMoves++;
            }
        }

        #endregion

        internal void UpdateTargetIndex(decimal[] targetSets)
        {
            LiftSet currentLiftSet = new LiftSet(this.LiftSets.FirstOrDefault());
            if (currentLiftSet != null)
            {
                if (currentLiftSet.Bar.TotalWeight == targetSets[this.CurrentTargetIndex + 1])
                {
                    this.CurrentTargetIndex = this.CurrentTargetIndex + 1;
                    if(this.CurrentTargetIndex + 1 <= targetSets.Count() - 1)
                        this.CurrentTargetWeight = targetSets[this.CurrentTargetIndex + 1];
                }
            }
        }

        internal bool AtFinalSet(decimal[] targetSets)
        {
            if (this.CurrentTargetIndex == targetSets.Count() - 1)
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

        #endregion

        #region Comparable

        //we need to sort first by smallest difference, then by the weight pile, then by the target state, then by fewest moves
        public int CompareTo(object obj)
        {
            if (obj == null)
                return -1;

            LiftSession session = obj as LiftSession;
            //return the session that has the lower difference, this is higher priority
            if (this.TargetDifference() > session.TargetDifference())
                return -1;
            else if (this.TargetDifference() < session.TargetDifference())
                return 1;
            else
            {
                //if the distance to the target is the same then session with smallest weight pile is higher priority
                if (this.PulledWeightStack.Count < session.PulledWeightStack.Count)
                    return -1;
                else if (this.PulledWeightStack.Count > session.PulledWeightStack.Count)
                    return 1;
                else
                {
                    //if the weight stack is the same then the one that's got the higher target state is higher priority
                    if (this.CurrentTargetIndex > session.CurrentTargetIndex)
                        return -1;
                    else if (this.CurrentTargetIndex < session.CurrentTargetIndex)
                        return 1;
                    else
                    {
                        //if the taret index is the same, the session with the lower movecount is higher priority
                        if (this.WeightSetMoves < session.WeightSetMoves)
                            return -1;
                        else if (this.WeightSetMoves > session.WeightSetMoves)
                            return 1;
                        else
                        {
                            //if move count is different then they are equally good (fack)
                        }
                    }
                }
            }

            return -1;
        }

        #endregion


    }
}
