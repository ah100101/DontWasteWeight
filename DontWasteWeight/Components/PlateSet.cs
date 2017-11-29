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

        private Plate[] plates;
        private decimal weight;
        private decimal totalWeight;

        #endregion

        #region Properties

        /// <summary>
        /// Array of plates (always 2)
        /// </summary>
        public Plate[] Plates
        {
            get
            {
                return plates;
            }
            set
            {
                plates = value;
            }
        }

        /// <summary>
        /// Total weight of PlateSet (Plate + Plate). Total added to Bar for TotalWeight
        /// </summary>
        public decimal TotalWeight
        {
            get
            {
                return totalWeight;
            }
            set
            {
                totalWeight = value;
            }
        }

        /// <summary>
        /// Identifying weight of PlateSet. Weight = 45, TotalWeight = 90.
        /// </summary>
        public decimal Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs empty PlateSet
        /// </summary>
        public PlateSet()
        {
            plates = new Plate[0];
            totalWeight = 0;
            weight = 0;
        }

        /// <summary>
        /// Constructrs PlateSet from existing
        /// </summary>
        /// <param name="plateSet">Existing PlateSet</param>
        public PlateSet(PlateSet plateSet)
        {
            this.plates = plateSet.Plates;
            this.weight = plateSet.Weight;
            this.totalWeight = plateSet.TotalWeight;
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
            weight = dec;
            totalWeight = weight + weight;

            Plate[] plates
                    = new Plate[]{ 
                                    new Plate(){ Weight = dec }, 
                                    new Plate(){ Weight = dec }
                                };

            this.plates = plates;
        }

        #endregion
    }
}
