using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DontWasteWeight.Components;

namespace WeightTests.Components
{
    [TestClass]
    public class BarTests
    {
        [TestMethod]
        public void EmptyConstructor_InitializesWeight_ToZero()
        {
            Bar bar = new Bar();

            Assert.AreEqual(0, bar.BarWeight);
            Assert.AreEqual(0, bar.TotalWeight);
            Assert.AreEqual(0, bar.LoadedPlates.Count);
        }

        [TestMethod]
        public void WeightParameterConstructor_InitializesWeight()
        {
            Bar bar = new Bar(45);

            Assert.AreEqual(45, bar.BarWeight);
            Assert.AreEqual(45, bar.TotalWeight);
            Assert.AreEqual(0, bar.LoadedPlates.Count);
        }

        [TestMethod]
        public void CopyConstructor_SetsMembers()
        {
            Bar bar1 = new Bar(45);
            Bar bar2 = new Bar(bar1);

            Assert.AreEqual(bar1.BarWeight, bar2.BarWeight);
            Assert.AreEqual(bar1.TotalWeight, bar2.TotalWeight);
            Assert.AreEqual(bar1.LoadedPlates.Count, bar2.LoadedPlates.Count);
        }
    }
}
