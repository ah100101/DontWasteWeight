using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    //TODO: Consolidate Bar class into LiftSet, too many layers with this.
    /// <summary>
    /// Tracks currently loaded weights and current loaded weight
    /// </summary>
    [Serializable]
    public class Bar
    {
        #region Members

        private decimal _barWeight;
        private decimal _totalWeight;
        private Stack<PlateSet> _loadedPlates;

        #endregion

        #region Properties

        /// <summary>
        /// Tracks the weight of the bar only
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
        /// Tracks the weight of the bar and all the loaded plates
        /// </summary>
        public decimal TotalWeight
        {
            get
            {
                return _totalWeight;
            }
            set
            {
                _totalWeight = value;
            }
        }

        /// <summary>
        /// Stack for keeping platesets (2 plates / 1 plateset)
        /// </summary>
        public Stack<PlateSet> LoadedPlates
        {
            get
            {
                return _loadedPlates;
            }
            set
            {
                _loadedPlates = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize everything to 0 and LoadedPlates to an empty PlateSet Stack
        /// </summary>
        public Bar()
        {
            _barWeight = 0;
            _totalWeight = 0;
            _loadedPlates = new Stack<PlateSet>();
        }

        /// <summary>
        /// Initializes Bar off of int weight. LoadedPlates set to an empty PlateSet Stack
        /// </summary>
        /// <param name="weight">Bar Weight</param>
        public Bar(int weight)
        {
            _barWeight = 45;
            _totalWeight = 45;
            _loadedPlates = new Stack<PlateSet>();
        }

        /// <summary>
        /// Initializes Bar of existing Bar
        /// </summary>
        /// <param name="bar">Existing Bar</param>
        public Bar(Bar bar)
        {
            _barWeight = bar.BarWeight;
            _totalWeight = bar.TotalWeight;
            _loadedPlates = bar._loadedPlates;
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Internal method for adding a PlateSet to Bar. Updates total weight.
        /// </summary>
        /// <param name="plateSet">PlateSet to add</param>
        internal void AddPlateSet(PlateSet plateSet)
        {
            _loadedPlates.Push(plateSet);

            Stack<PlateSet> totalPlates = new Stack<PlateSet>(_loadedPlates);
            decimal totalPlateWeight = 0;

            while (totalPlates.Count > 0)
            {
                PlateSet set = totalPlates.Pop();
                totalPlateWeight = totalPlateWeight + set.TotalWeight;
            }

            _totalWeight = totalPlateWeight + _barWeight;
        }

        /// <summary>
        /// Internal method for removing plates from bar. Updates total weight.
        /// </summary>
        /// <param name="setsToRemove">Number of PlateSets to remove</param>
        /// <returns></returns>
        internal List<PlateSet> RemovePlates(int setsToRemove)
        {
            List<PlateSet> plateSetsRemoved = new List<PlateSet>();

            if (setsToRemove <= _loadedPlates.Count)
            {
                for (int i = 0; i < setsToRemove; i++)
                {
                    plateSetsRemoved.Add(_loadedPlates.Pop());
                }

                if (_loadedPlates.Count > 0)
                {
                    decimal newTotalWeight = _barWeight;
                    Stack<PlateSet> addingStack = new Stack<PlateSet>(_loadedPlates);

                    while (addingStack.Count > 0)
                    {
                        newTotalWeight = newTotalWeight + addingStack.Pop().TotalWeight;
                    }

                    _totalWeight = newTotalWeight;
                }
                else
                {
                    _totalWeight = _barWeight;
                }
            }

            return plateSetsRemoved;
        }

        #endregion
    }
}
