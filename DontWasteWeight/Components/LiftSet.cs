﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontWasteWeight.Core.Utilities;

namespace DontWasteWeight.Components
{
    [Serializable]
    public class LiftSet
    {
        private Bar bar;

        #region Properties

        /// <summary>
        /// Bar being loaded with weight
        /// </summary>
        public Bar Bar
        {
            get
            {
                return bar;
            }
            set
            {
                bar = value;
            }
        }

        /// <summary>
        /// Weight of bar and all loaded plates
        /// </summary>
        public decimal TotalWeight
        {
            get
            {
                return bar.TotalWeight;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct LiftSet with new bar (no weight set)
        /// </summary>
        public LiftSet()
        {
            bar = new Bar();
        }

        /// <summary>
        /// Construct LiftSet from existing
        /// </summary>
        /// <param name="liftSet">Existing LiftSet</param>
        public LiftSet(LiftSet liftSet)
        {
            this.bar = Cloner.Clone(liftSet.Bar);
        }

        #endregion

        /// <summary>
        /// Loads PlateSet to bar and updates weight
        /// </summary>
        /// <param name="plateSet">PlateSet to load</param>
        public void AddPlateSetToBar(PlateSet plateSet)
        {
            bar.AddPlateSet(plateSet);
        }

        /// <summary>
        /// Determines if there are loaded plates that can be removed
        /// </summary>
        /// <returns>True if Bar has loaded plates</returns>
        public bool CanRemovePlates()
        {
            if (bar != null && bar.LoadedPlates != null && bar.LoadedPlates.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Strips plates from bar
        /// </summary>
        /// <param name="setsToRemove">Number of PlateSets to remove</param>
        public void RemovePlates(int setsToRemove)
        {
            Bar.RemovePlates(setsToRemove);
        }
    }
}
