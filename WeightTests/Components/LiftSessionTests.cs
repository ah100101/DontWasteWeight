using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DontWasteWeight.Components;
using System.Collections.Generic;

namespace WeightTests.Components
{
    [TestClass]
    public class LiftSessionTests
    {
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
    }
}
