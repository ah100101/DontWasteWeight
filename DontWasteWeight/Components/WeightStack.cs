using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    /// <summary>
    /// Tracks a stack of PlateSets. Identified by weight (45s, 35s, etc)
    /// </summary>
    [Serializable]
    public class WeightStack
    {
        #region Members

        private decimal weight;
        private Stack<PlateSet> plates;

        #endregion

        #region Properties

        /// <summary>
        /// Plate being tracked
        /// </summary>
        public Stack<PlateSet> Plates
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
        /// Identifying weight for this stack
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

        #region Methods

        /// <summary>
        /// Fill an existing WeighStack by the weight and number of PlateSets
        /// </summary>
        /// <param name="weight">Weight to fill</param>
        /// <param name="weightSetCount">Number of PlateSets to add</param>
        public void Fill(decimal weight, int weightSetCount)
        {
            if (this.plates == null)
                this.plates = new Stack<PlateSet>();

            if(weight > 0 && weightSetCount >= 0)
            {
                this.Weight = weight;

                for (int i = 0; i < weightSetCount; i++)
                {
                    PlateSet set = new PlateSet();
                    Plate[] plates 
                        = new Plate[]{ 
                                        new Plate(){ Weight = weight }, 
                                        new Plate(){ Weight = weight }
                                    };

                    set.Plates = plates;
                    set.TotalWeight = weight + weight;

                    this.plates.Push(set);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes Plate to new PlateSet stack
        /// </summary>
        public WeightStack()
        {
            plates = new Stack<PlateSet>();
        }

        /// <summary>
        /// Constructs WeighStack from existing
        /// </summary>
        /// <param name="weightStack">Existing WeightStack</param>
        public WeightStack(WeightStack weightStack)
        {
            this.plates = weightStack.Plates;
            this.weight = weightStack.Weight;
        }

        /// <summary>
        /// Constructs WeightStack from a given weight
        /// </summary>
        /// <param name="weight">Identifying Weight</param>
        public WeightStack(decimal weight)
        {
            Fill(weight, 1);
        }

        /// <summary>
        /// Constructs WeightStack from weight and size
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="setCount"></param>
        public WeightStack(decimal weight, int setCount)
        {
            Fill(weight, setCount);
        }

        #endregion
    }
}
