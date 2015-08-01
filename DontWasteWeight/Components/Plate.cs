using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    [Serializable]
    public class Plate
    {
        private decimal _weight;

        #region Properties

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

        public Plate()
        {
            _weight = 0;
        }

        #endregion
    }
}
