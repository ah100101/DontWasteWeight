using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    [Serializable]
    public class Bar
    {
        private decimal _barWeight;
        private decimal _totalWeight;
        private Stack<PlateSet> _loadedPlates;

        #region Properties

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

        public Bar()
        {
            _barWeight = 0;
            _totalWeight = 0;
            _loadedPlates = new Stack<PlateSet>();
        }

        public Bar(Bar bar)
        {
            _barWeight = bar.BarWeight;
            _totalWeight = bar.TotalWeight;
            _loadedPlates = bar._loadedPlates;
        }

        #endregion 

        #region Methods

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

        internal void RemovePlates(int setsToRemove)
        {
            if (setsToRemove <= _loadedPlates.Count)
            {
                for (int i = 0; i < setsToRemove; i++)
                {
                    _loadedPlates.Pop();
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
        }

        #endregion
    }
}
