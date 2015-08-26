using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DontWasteWeight.Components;
using System.Collections.Generic;

namespace WeightTests.Components
{
    [TestClass]
    public class LiftSessionTests
    {
        #region Constructor Tests

        [TestCategory("LiftSession"), TestMethod]
        public void DefaultConstructor_Initializes_SearchMembers()
        {
            LiftSession session = new LiftSession();
            Assert.AreEqual(new Stack<LiftSet>().Count, session.LiftSets.Count);
            Assert.AreEqual(new Stack<WeightStack>().Count, session.PulledWeightStacks.Count);
            Assert.AreEqual(0, session.Targets.Length);
            Assert.AreEqual(0, session.WeightSetMoves);
            Assert.AreEqual(0, session.BarWeight);
            Assert.AreEqual(0, session.UsedPlatesCount);
            Assert.AreEqual(-1, session.CurrentTargetIndex);
            Assert.AreEqual(-1, session.CurrentTargetWeight);
            Assert.AreEqual(0, session.LiftSets.Count);
            Assert.AreEqual(0, session.PulledWeightStacks.Count);
        }

        [TestCategory("LiftSession"), TestMethod]
        public void CopyConstructor_Copies_AllSearchMembers_ForInitializedSession()
        {
            LiftSession session1 = new LiftSession();
            LiftSession session2 = new LiftSession(session1);

            Assert.AreEqual(session1.LiftSets.Count, session2.LiftSets.Count);
            Assert.AreEqual(session1.PulledWeightStacks.Count, session2.PulledWeightStacks.Count);
            Assert.AreEqual(session1.Targets, session2.Targets);
            Assert.AreEqual(session1.WeightSetMoves, session2.WeightSetMoves);
            Assert.AreEqual(session1.BarWeight, session2.BarWeight);
            Assert.AreEqual(session1.UsedPlatesCount, session2.UsedPlatesCount);
            Assert.AreEqual(session1.CurrentTargetIndex, session2.CurrentTargetIndex);
            Assert.AreEqual(session1.CurrentTargetWeight, session2.CurrentTargetWeight);
            Assert.AreEqual(session1.LiftSets.Count, session2.LiftSets.Count);
            Assert.AreEqual(session1.PulledWeightStacks.Count, session2.PulledWeightStacks.Count);
        }

        [TestCategory("LiftSession"), TestMethod]
        public void CopyConstructor_Copies_AllSearchMembers_ForSetSession()
        {
            LiftSession session1 = new LiftSession();

            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 165;
            targetSets[1] = 185;

            session1.BarWeight = 45;
            session1.WeightSetMoves = 0;
            session1.SessionWeightStacks = CreateGymWeightStacks();
            session1.CreateBaseSet();
            session1.CurrentTargetIndex = 0;
            session1.CurrentTargetWeight = targetSets[0];
            session1.UpdateTargetIndex(targetSets);
            session1.Targets = targetSets;

            LiftSet liftSet = new LiftSet();
            liftSet.Bar.BarWeight = 45;
            liftSet.Bar.TotalWeight = 45;
            session1.LiftSets.Push(liftSet);

            LiftSession session2 = new LiftSession(session1);

            Assert.AreEqual(session1.LiftSets.Count, session2.LiftSets.Count);
            Assert.AreEqual(session1.PulledWeightStacks.Count, session2.PulledWeightStacks.Count);
            Assert.AreEqual(session1.Targets, session2.Targets);
            Assert.AreEqual(session1.WeightSetMoves, session2.WeightSetMoves);
            Assert.AreEqual(session1.BarWeight, session2.BarWeight);
            Assert.AreEqual(session1.UsedPlatesCount, session2.UsedPlatesCount);
            Assert.AreEqual(session1.CurrentTargetIndex, session2.CurrentTargetIndex);
            Assert.AreEqual(session1.CurrentTargetWeight, session2.CurrentTargetWeight);
            Assert.AreEqual(session1.PulledWeightStacks.Count, session2.PulledWeightStacks.Count);
            Assert.AreEqual(session1.LiftSets.Peek().Bar.BarWeight, session2.LiftSets.Peek().Bar.BarWeight);
            Assert.AreEqual(session1.LiftSets.Peek().Bar.TotalWeight, session2.LiftSets.Peek().Bar.TotalWeight);
            Assert.AreEqual(session1.LiftSets.Peek().Bar.LoadedPlates.Count, session2.LiftSets.Peek().Bar.LoadedPlates.Count);
        }

        [TestCategory("LiftSession"), TestMethod]
        public void CopyConstructor_Copies_MaintainsClone_ForSetSession()
        {
            LiftSession session1 = new LiftSession();

            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 165;
            targetSets[1] = 185;

            session1.BarWeight = 45;
            session1.WeightSetMoves = 0;
            session1.SessionWeightStacks = CreateGymWeightStacks();
            session1.CreateBaseSet();
            session1.CurrentTargetIndex = 0;
            session1.CurrentTargetWeight = targetSets[0];
            session1.UpdateTargetIndex(targetSets);
            session1.Targets = targetSets;

            LiftSet liftSet = new LiftSet();
            liftSet.Bar.BarWeight = 45;
            liftSet.Bar.TotalWeight = 45;
            session1.LiftSets.Push(liftSet);

            LiftSession session2 = new LiftSession(session1);

            LiftSet liftSet2 = new LiftSet();
            liftSet2.Bar.BarWeight = 50;
            liftSet2.Bar.TotalWeight = 50;
            liftSet2.Bar.LoadedPlates.Push(new PlateSet(45));

            session2.LiftSets.Push(liftSet2);
            session2.PulledWeightStacks.Add(new WeightStack());
            session2.WeightSetMoves = session2.WeightSetMoves + 1;
            session2.BarWeight = 50;
            session2.UsedPlatesCount = session2.UsedPlatesCount + 1;
            session2.CurrentTargetIndex = 2;
            session2.CurrentTargetWeight = 185;
            session2.PulledWeightStacks.Add(new WeightStack());

            Assert.AreNotEqual(session1.LiftSets.Count, session2.LiftSets.Count);
            Assert.AreNotEqual(session1.PulledWeightStacks.Count, session2.PulledWeightStacks.Count);
            Assert.AreEqual(session1.Targets, session2.Targets);
            Assert.AreNotEqual(session1.WeightSetMoves, session2.WeightSetMoves);
            Assert.AreNotEqual(session1.BarWeight, session2.BarWeight);
            Assert.AreNotEqual(session1.UsedPlatesCount, session2.UsedPlatesCount);
            Assert.AreNotEqual(session1.CurrentTargetIndex, session2.CurrentTargetIndex);
            Assert.AreNotEqual(session1.CurrentTargetWeight, session2.CurrentTargetWeight);
            Assert.AreNotEqual(session1.PulledWeightStacks.Count, session2.PulledWeightStacks.Count);
            Assert.AreNotEqual(session1.LiftSets.Peek().Bar.BarWeight, session2.LiftSets.Peek().Bar.BarWeight);
            Assert.AreNotEqual(session1.LiftSets.Peek().Bar.TotalWeight, session2.LiftSets.Peek().Bar.TotalWeight);
            Assert.AreNotEqual(session1.LiftSets.Peek().Bar.LoadedPlates.Count, session2.LiftSets.Peek().Bar.LoadedPlates.Count);
        }

        #endregion

        #region Initialization Tests

        [TestCategory("LiftSession"), TestMethod]
        public void BaseLiftSet_CreatedFrom_CreateBaseSet()
        {
            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CreateBaseSet();

            Assert.AreEqual(1, session.LiftSets.Count);
            Assert.AreEqual(45, session.LiftSets.Peek().Bar.BarWeight);
            Assert.AreEqual(45, session.LiftSets.Peek().Bar.TotalWeight);
        }

        #endregion

        #region Adding Plate Tests

        [TestCategory("LiftSession"), TestMethod]
        public void PlatesNotAdded_WhenNoSessionStacks()
        {
            LiftSession session = new LiftSession();

            Assert.IsFalse(session.CanAddPlates());
            Assert.IsFalse(session.CanAddThesePlates(new PlateSet(45)));

            session.AddPlates(new PlateSet(45));

            Assert.AreEqual(0, session.PulledWeightStacks.Count);
            Assert.AreEqual(0, session.SessionWeightStacks.Count);
        }

        [TestCategory("LiftSession"), TestMethod]
        public void PlatesAdded_WhenSessionStacksAvailable()
        {
            LiftSession session = new LiftSession();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 1);
            session.SessionWeightStacks.Add(fortyFives);

            Assert.IsTrue(session.CanAddPlates());
            Assert.IsTrue(session.CanAddThesePlates(new PlateSet(45)));

            session.AddPlates(new PlateSet(45));

            Assert.AreEqual(1, session.PulledWeightStacks.Count);
            Assert.AreEqual(1, session.SessionWeightStacks.Count);
            Assert.AreEqual(1, session.PulledWeightStacks[0].Plates.Count);
            Assert.AreEqual(0, session.SessionWeightStacks[0].Plates.Count);
        }

        #endregion

        #region Removing Plate Tests

        [TestCategory("LiftSession"), TestMethod]
        public void PlatesNotRemoved_WhenNoSessionStacks()
        {
            LiftSession session = new LiftSession();

            Assert.IsFalse(session.CanRemovePlates());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void PlatesNotRemoved_WhenNoLiftSets()
        {
            LiftSession session = new LiftSession();
            session.StripPlates(2);
        }

        [TestCategory("LiftSession"), TestMethod]
        public void PlatesNotRemoved_WhenNoPlatesLoaded()
        {
            LiftSession session = new LiftSession();
            session.CreateBaseSet();
            session.StripPlates(2);
        }

        [TestCategory("LiftSession"), TestMethod]
        public void PlatesRemoved_WhenLoaded()
        {
            LiftSession session = new LiftSession();
            session.CreateBaseSet();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            session.AddPlates(new PlateSet(45));
            session.AddPlates(new PlateSet(45));
            session.AddPlates(new PlateSet(45));

            Assert.AreEqual(3, session.LiftSets.Peek().Bar.LoadedPlates.Count);

            session.StripPlates(2);

            Assert.AreEqual(1, session.LiftSets.Peek().Bar.LoadedPlates.Count);

            session.StripPlates(1);

            Assert.AreEqual(0, session.LiftSets.Peek().Bar.LoadedPlates.Count);
        }

        #endregion

        #region Heuristic Methods

        [TestCategory("LiftSession"), TestMethod]
        public void GnZero_WithNoPlates()
        {
            LiftSession session = new LiftSession();
            session.CreateBaseSet();

            Assert.AreEqual(0, session.Gn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void GnSameAsLoadedPlates()
        {
            LiftSession session = new LiftSession();
            session.CreateBaseSet();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            session.AddPlates(new PlateSet(45));
            session.AddPlates(new PlateSet(45));
            session.AddPlates(new PlateSet(45));

            Assert.AreEqual(3, session.Gn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void GnSameAsLoadedPlates_ThenStrippedOff()
        {
            LiftSession session = new LiftSession();
            session.CreateBaseSet();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            session.AddPlates(new PlateSet(45));
            session.AddPlates(new PlateSet(45));
            session.AddPlates(new PlateSet(45));

            session.StripPlates(3);

            Assert.AreEqual(3, session.Gn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void HnValue_ReflectsTargetDifferences()
        {
            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 135;
            targetSets[1] = 225;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 0;
            session.CurrentTargetWeight = targetSets[0];
            session.Targets = targetSets;
            session.CreateBaseSet();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            Assert.AreEqual(2, session.Hn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void HnValue_ReflectsTargetDifferences_DifferenceX2()
        {
            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 135;
            targetSets[1] = 315;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 0;
            session.CurrentTargetWeight = targetSets[0];
            session.Targets = targetSets;
            session.CreateBaseSet();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            Assert.AreEqual(3, session.Hn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void HnValue_DoesNotReflect_CurrentLocation()
        {
            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 135;
            targetSets[1] = 315;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 1;
            session.CurrentTargetWeight = targetSets[1];
            session.Targets = targetSets;
            session.CreateBaseSet();

            LiftSet nextLiftSet = new LiftSet();
            nextLiftSet.Bar.BarWeight = 45;
            nextLiftSet.Bar.LoadedPlates.Push(new PlateSet(45));
            nextLiftSet.Bar.TotalWeight = 135;

            session.LiftSets.Push(nextLiftSet);

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            Assert.AreEqual(2, session.Hn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void HnValue_Zero_AtLastTarget()
        {
            int numSets = 1;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 135;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 1;
            session.CurrentTargetWeight = targetSets[0];
            session.Targets = targetSets;
            session.CreateBaseSet();

            LiftSet nextLiftSet = new LiftSet();
            nextLiftSet.Bar.BarWeight = 45;
            nextLiftSet.Bar.LoadedPlates.Push(new PlateSet(45));
            nextLiftSet.Bar.TotalWeight = 135;

            session.LiftSets.Push(nextLiftSet);

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            Assert.AreEqual(0, session.Hn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void FnValue_Combining_PreviousAndForward()
        {
            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 135;
            targetSets[1] = 315;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 1;
            session.CurrentTargetWeight = targetSets[1];
            session.Targets = targetSets;
            session.CreateBaseSet();

            LiftSet nextLiftSet = new LiftSet();
            nextLiftSet.Bar.BarWeight = 45;
            nextLiftSet.Bar.LoadedPlates.Push(new PlateSet(45));
            nextLiftSet.Bar.TotalWeight = 135;

            session.LiftSets.Push(nextLiftSet);

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            Assert.AreEqual(3, session.Fn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void FnValue_OnlyGn_AtFinalTarget()
        {
            int numSets = 1;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 135;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 1;
            session.CurrentTargetWeight = targetSets[0];
            session.Targets = targetSets;
            session.CreateBaseSet();

            LiftSet nextLiftSet = new LiftSet();
            nextLiftSet.Bar.BarWeight = 45;
            nextLiftSet.Bar.LoadedPlates.Push(new PlateSet(45));
            nextLiftSet.Bar.TotalWeight = 135;

            session.LiftSets.Push(nextLiftSet);

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            Assert.AreEqual(1, session.Fn());
        }

        [TestCategory("LiftSession"), TestMethod]
        public void FnValue_OnlyHn_AtStart()
        {
            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 135;
            targetSets[1] = 315;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 0;
            session.CurrentTargetWeight = targetSets[0];
            session.Targets = targetSets;
            session.CreateBaseSet();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            Assert.AreEqual(3, session.Fn());
        }

        #endregion

        #region Expanding

        [TestCategory("LiftSession"), TestMethod]
        public void ExpansionsCount_Matches_SessionStacks_AtStart()
        {
            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 225;
            targetSets[1] = 315;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 0;
            session.CurrentTargetWeight = targetSets[0];
            session.Targets = targetSets;
            session.CreateBaseSet();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            WeightStack thirtyFives = new WeightStack();
            thirtyFives.Fill(35, 10);
            session.SessionWeightStacks.Add(thirtyFives);

            Assert.AreEqual(2, session.Expand().Length);
        }

        [TestCategory("LiftSession"), TestMethod]
        public void ExpansionsCount_Matches_SessionStacks_PlusStrip_WhenLoaded()
        {
            int numSets = 2;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 225;
            targetSets[1] = 315;

            LiftSession session = new LiftSession();
            session.BarWeight = 45;
            session.CurrentTargetIndex = 0;
            session.CurrentTargetWeight = targetSets[0];
            session.Targets = targetSets;
            session.CreateBaseSet();

            LiftSet nextLiftSet = new LiftSet();
            nextLiftSet.Bar.BarWeight = 45;
            nextLiftSet.Bar.LoadedPlates.Push(new PlateSet(45));
            nextLiftSet.Bar.TotalWeight = 135;
            session.LiftSets.Push(nextLiftSet);

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 10);
            session.SessionWeightStacks.Add(fortyFives);

            WeightStack thirtyFives = new WeightStack();
            thirtyFives.Fill(35, 10);
            session.SessionWeightStacks.Add(thirtyFives);

            Assert.AreEqual(3, session.Expand().Length);
        }

        #endregion

        #region Equivalent Tests

        [TestCategory("LiftSession"), TestMethod]
        public void NotEquivalent_IndexDifferent()
        {
            LiftSession session1 = new LiftSession();
            session1.CurrentTargetIndex = 1;
            LiftSession session2 = new LiftSession();

            Assert.IsFalse(session1.IsEquivalentNode(session2));
        }

        [TestCategory("LiftSession"), TestMethod]
        public void NotEquivalent_WeightDifferent()
        {
            LiftSession session1 = new LiftSession();
            session1.BarWeight = 45;
            session1.CreateBaseSet();
            

            LiftSession session2 = new LiftSession();
            session2.BarWeight = 35;
            session2.CreateBaseSet();

            Assert.IsFalse(session1.IsEquivalentNode(session2));
        }

        [TestCategory("LiftSession"), TestMethod]
        public void Equivalent_IndexAndWeightSame()
        {
            LiftSession session1 = new LiftSession();
            session1.BarWeight = 45;
            session1.CurrentTargetIndex = 1;
            session1.CreateBaseSet();


            LiftSession session2 = new LiftSession();
            session2.BarWeight = 45;
            session2.CurrentTargetIndex = 1;
            session2.CreateBaseSet();

            Assert.IsTrue(session1.IsEquivalentNode(session2));
        }

        #endregion

        #region Helper Methods

        private static List<WeightStack> CreateGymWeightStacks()
        {
            List<WeightStack> weightStacks = new List<WeightStack>();

            WeightStack fortyFives = new WeightStack();
            fortyFives.Fill(45, 20);
            weightStacks.Add(fortyFives);

            WeightStack twentyFives = new WeightStack();
            twentyFives.Fill(25, 20);
            weightStacks.Add(twentyFives);

            WeightStack fifteens = new WeightStack();
            fifteens.Fill(15, 20);
            weightStacks.Add(fifteens);

            WeightStack tens = new WeightStack();
            tens.Fill(10, 20);
            weightStacks.Add(tens);

            WeightStack fives = new WeightStack();
            fives.Fill(5, 20);
            weightStacks.Add(fives);

            return weightStacks;
        }

        #endregion
    }
}
