using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    //TODO: Place all bar members and methods on this class. Unclutter the hiearchy
    /// <summary>
    /// Comprised of two plates. PlateSets are added to a Bar to increase its total weight.
    /// </summary>
    [Serializable]
    public class PlateSet
    {
        #region Members

        private Plate[] _plates;
        private decimal _weight;
        private decimal _totalWeight;

        #endregion

        #region Properties

        /// <summary>
        /// Array of plates (always 2)
        /// </summary>
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

        /// <summary>
        /// Total weight of PlateSet (Plate + Plate). Total added to Bar for TotalWeight
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
        /// Identifying weight of PlateSet. Weight = 45, TotalWeight = 90.
        /// </summary>
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

        /// <summary>
        /// Constructs empty PlateSet
        /// </summary>
        public PlateSet()
        {
            _plates = new Plate[0];
            _totalWeight = 0;
            _weight = 0;
        }

        /// <summary>
        /// Constructrs PlateSet from existing
        /// </summary>
        /// <param name="plateSet">Existing PlateSet</param>
        public PlateSet(PlateSet plateSet)
        {
            this._plates = plateSet.Plates;
            this._weight = plateSet.Weight;
            this._totalWeight = plateSet.TotalWeight;
        }

        /// <summary>
        /// Constructs PlateSet from an identifying Weight. Creates 2 plate with this weight.
        /// </summary>
        /// <param name="weight">Identifying weight</param>
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

        //TODO: Remove this, as this was a hotfix for different issues
        /// <summary>
        /// Creates plates in case they were already added
        /// </summary>
        /// <param name="dec">Identifying weight</param>
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
