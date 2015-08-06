using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    [Serializable]
    public class WeightStack
    {
        private decimal _weight;
        private Stack<PlateSet> _plates;

        #region Properties

        public Stack<PlateSet> Plates
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

        #region Methods

        public void Fill(decimal weight, int weightSetCount)
        {
            if (this._plates == null)
                this._plates = new Stack<PlateSet>();

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

                    this._plates.Push(set);
                }
            }
        }

        #endregion

        #region Constructors

        public WeightStack()
        {
            _plates = new Stack<PlateSet>();
        }

        public WeightStack(WeightStack weightStack)
        {
            this._plates = weightStack.Plates;
            this._weight = weightStack.Weight;
        }

        public WeightStack(decimal weight)
        {
            Fill(weight, 1);
        }

        public WeightStack(decimal weight, int setCount)
        {
            Fill(weight, setCount);
        }

        #endregion
    }
}
