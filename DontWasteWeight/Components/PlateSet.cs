using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    [Serializable]
    public class PlateSet
    {
        private Plate[] _plates;
        private decimal _weight;
        private decimal _totalWeight;

        #region Properties

        public Plate[] Plates
        {
            get
            {
                return _plates;
            }
            set
            {
                _plates = value;
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

        public decimal Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }

        #endregion

        #region Constructors

        public PlateSet()
        {
            _plates = new Plate[0];
            _totalWeight = 0;
            _weight = 0;
        }

        public PlateSet(PlateSet plateSet)
        {
            this._plates = plateSet.Plates;
            this._weight = plateSet.Weight;
            this._totalWeight = plateSet.TotalWeight;
        }

        public PlateSet(int weight)
        {
            Plate[] plates
                        = new Plate[]{
                                        new Plate(){ Weight = weight },
                                        new Plate(){ Weight = weight }
                                    };

            Plates = plates;
            TotalWeight = weight + weight;
            Weight = weight;
        }

        #endregion

        #region Methods

        internal void InitializePlates(decimal dec)
        {
            _weight = dec;
            _totalWeight = _weight + _weight;

            Plate[] plates
                    = new Plate[]{ 
                                    new Plate(){ Weight = dec }, 
                                    new Plate(){ Weight = dec }
                                };

            _plates = plates;
        }

        #endregion
    }
}
