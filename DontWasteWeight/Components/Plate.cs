using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    //TODO: Get rid of this class and bake it into PlateSet class. Simplify hiearchy.
    /// <summary>
    /// Lowest level class in hiearchy. 2 plates belong to PlateSet
    /// </summary>
    [Serializable]
    public class Plate
    {
        #region Members

        private decimal _weight;

        #endregion

        #region Properties

        /// <summary>
        /// Weight of plates
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
        /// Construct with weight set to 0
        /// </summary>
        public Plate()
        {
            _weight = 0;
        }

        #endregion
    }
}
