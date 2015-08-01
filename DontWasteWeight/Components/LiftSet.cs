using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Components
{
    [Serializable]
    public class LiftSet
    {
        private Bar _bar;

        #region Properties

        public Bar Bar
        {
            get
            {
                return _bar;
            }
            set
            {
                _bar = value;
            }
        }

        #endregion

        #region Constructors

        public LiftSet()
        {
            _bar = new Bar();
        }

        public LiftSet(LiftSet liftSet)
        {
            this._bar = new Bar(liftSet.Bar);
        }

        #endregion

        internal void AddPlateSetToBar(PlateSet plateSet)
        {
            _bar.AddPlateSet(plateSet);
        }

        internal bool CanRemovePlates()
        {
            if (_bar != null && _bar.LoadedPlates != null && _bar.LoadedPlates.Count > 0)
                return true;
            return false;
        }
    }
}
