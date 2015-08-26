using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axel.Utilities;

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

        public decimal TotalWeight
        {
            get
            {
                return _bar.TotalWeight;
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
            this._bar = Cloner.Clone(liftSet.Bar);
        }

        #endregion

        public void AddPlateSetToBar(PlateSet plateSet)
        {
            _bar.AddPlateSet(plateSet);
        }

        public bool CanRemovePlates()
        {
            if (_bar != null && _bar.LoadedPlates != null && _bar.LoadedPlates.Count > 0)
                return true;
            return false;
        }

        public void RemovePlates(int setsToRemove)
        {
            Bar.RemovePlates(setsToRemove);
        }
    }
}
