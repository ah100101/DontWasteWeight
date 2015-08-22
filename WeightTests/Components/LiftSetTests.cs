using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DontWasteWeight.Components;

namespace WeightTests.Components
{
    [TestClass]
    public class LiftSetTests
    {
        [TestMethod]
        public void AddingPlateSet_ToEmptyBar_UpdatesTotalWeight()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);
            PlateSet plateSet = new PlateSet(45);

            liftSet.Bar = bar;
            liftSet.AddPlateSetToBar(plateSet);

            Assert.AreEqual(135, liftSet.TotalWeight);
        }

        [TestMethod]
        public void AddingPlateSets_ToEmptyBar_UpdatesTotalWeight()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);
            PlateSet plateSet1 = new PlateSet(45);
            PlateSet plateSet2 = new PlateSet(25);
            PlateSet plateSet3 = new PlateSet(10);

            liftSet.Bar = bar;
            liftSet.AddPlateSetToBar(plateSet1);
            liftSet.AddPlateSetToBar(plateSet2);
            liftSet.AddPlateSetToBar(plateSet3);

            Assert.AreEqual(205, liftSet.TotalWeight);
        }

        [TestMethod]
        public void CanRemovePlates_ReturnsTrue_WithLoadedBar()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);
            PlateSet plateSet1 = new PlateSet(45);

            liftSet.Bar = bar;
            liftSet.AddPlateSetToBar(plateSet1);

            Assert.AreEqual(true, liftSet.CanRemovePlates());
        }

        [TestMethod]
        public void CanRemovePlates_ReturnsFalse_WithEmptyBar()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);

            liftSet.Bar = bar;

            Assert.AreEqual(false, liftSet.CanRemovePlates());
        }

        [TestMethod]
        public void RemovingAllPlateSets_SetsTotalWeight_ToBarWeight()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);
            PlateSet plateSet1 = new PlateSet(45);
            PlateSet plateSet2 = new PlateSet(25);
            PlateSet plateSet3 = new PlateSet(10);

            liftSet.Bar = bar;
            liftSet.AddPlateSetToBar(plateSet1);
            liftSet.AddPlateSetToBar(plateSet2);
            liftSet.AddPlateSetToBar(plateSet3);
            liftSet.RemovePlates(3);

            Assert.AreEqual(45, liftSet.TotalWeight);
        }

        [TestMethod]
        public void RemovingSomePlateSets_UpdatesTotalWeight()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);
            PlateSet plateSet1 = new PlateSet(45);
            PlateSet plateSet2 = new PlateSet(25);
            PlateSet plateSet3 = new PlateSet(10);

            liftSet.Bar = bar;
            liftSet.AddPlateSetToBar(plateSet1);
            liftSet.AddPlateSetToBar(plateSet2);
            liftSet.AddPlateSetToBar(plateSet3);
            liftSet.RemovePlates(2);

            Assert.AreEqual(135, liftSet.TotalWeight);
        }

        [TestMethod]
        public void CanRemovePlates_ReturnsFalse_WithUnloadedBar()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);
            PlateSet plateSet1 = new PlateSet(45);
            PlateSet plateSet2 = new PlateSet(25);
            PlateSet plateSet3 = new PlateSet(10);

            liftSet.Bar = bar;
            liftSet.AddPlateSetToBar(plateSet1);
            liftSet.AddPlateSetToBar(plateSet2);
            liftSet.AddPlateSetToBar(plateSet3);
            liftSet.RemovePlates(3);

            Assert.AreEqual(false, liftSet.CanRemovePlates());
        }

        [TestMethod]
        public void CopyConstructor_CopiesAllBarValues()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);
            PlateSet plateSet1 = new PlateSet(45);
            PlateSet plateSet2 = new PlateSet(25);
            PlateSet plateSet3 = new PlateSet(10);

            liftSet.Bar = bar;
            liftSet.AddPlateSetToBar(plateSet1);
            liftSet.AddPlateSetToBar(plateSet2);
            liftSet.AddPlateSetToBar(plateSet3);

            LiftSet liftSetCopy = new LiftSet(liftSet);

            Assert.AreEqual(liftSet.TotalWeight, liftSetCopy.TotalWeight);
            Assert.AreEqual(liftSet.Bar.BarWeight, liftSetCopy.Bar.BarWeight);
            Assert.AreEqual(liftSet.Bar.LoadedPlates.Count, liftSetCopy.Bar.LoadedPlates.Count);
            Assert.AreEqual(liftSet.Bar.LoadedPlates.Peek().Weight, liftSetCopy.Bar.LoadedPlates.Peek().Weight);
        }

        [TestMethod]
        public void CopyConstructor_MaintainsClone()
        {
            LiftSet liftSet = new LiftSet();
            Bar bar = new Bar(45);
            PlateSet plateSet1 = new PlateSet(45);
            PlateSet plateSet2 = new PlateSet(25);
            PlateSet plateSet3 = new PlateSet(10);

            liftSet.Bar = bar;
            liftSet.AddPlateSetToBar(plateSet1);
            liftSet.AddPlateSetToBar(plateSet2);
            liftSet.AddPlateSetToBar(plateSet3);

            LiftSet liftSetCopy = new LiftSet(liftSet);

            liftSetCopy.Bar.BarWeight = 35;
            liftSetCopy.RemovePlates(1);

            Assert.AreNotEqual(liftSet.TotalWeight, liftSetCopy.TotalWeight);
            Assert.AreNotEqual(liftSet.Bar.BarWeight, liftSetCopy.Bar.BarWeight);
            Assert.AreNotEqual(liftSet.Bar.LoadedPlates.Count, liftSetCopy.Bar.LoadedPlates.Count);
            Assert.AreNotEqual(liftSet.Bar.LoadedPlates.Peek().Weight, liftSetCopy.Bar.LoadedPlates.Peek().Weight);
            Assert.AreEqual(175, liftSetCopy.TotalWeight);
        }
    }
}
