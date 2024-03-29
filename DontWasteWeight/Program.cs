﻿using DontWasteWeight.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontWasteWeight.Core.Data.Structures;
using DontWasteWeight.Core.Data.Search;
using DontWasteWeight.Core.Algorithms.Search.Generic;
using DontWasteWeight.Core.Utilities;

namespace DontWasteWeight
{
    // Class that just runs a scenario of the search
    class Program
    {
        static void Main(string[] args)
        {
            //Create all the necessary stacks of weights available for use
            List<WeightStack> startingWeightStacks = CreateGymWeightStacks();

            //Create the array of target sets that need to be hit
            int numSets = 6;
            decimal[] targetSets = new decimal[numSets];
            targetSets[0] = 165;
            targetSets[1] = 185;
            targetSets[2] = 205;
            targetSets[3] = 225;
            targetSets[4] = 245;
            targetSets[5] = 265;
            
            //session for starting
            LiftSession originSession = new LiftSession();

            //session for finishing
            LiftSession targetSession = new LiftSession();


            //TODO: Simplify initialization process into single method cll
            //initialize origin session
            originSession.BarWeight = 45;
            originSession.WeightSetMoves = 0;
            originSession.SessionWeightStacks = startingWeightStacks;
            originSession.CreateBaseSet();
            originSession.CurrentTargetIndex = 0;
            originSession.CurrentTargetWeight = targetSets[0];
            originSession.UpdateTargetIndex(targetSets);
            originSession.Targets = targetSets;

            //initialize target session
            targetSession = Cloner.Clone(originSession);
            targetSession.CurrentTargetIndex = targetSession.Targets.Count() - 1;
            targetSession.CurrentTargetWeight = targetSession.Targets[targetSession.CurrentTargetIndex];
            LiftSet targetSet = new LiftSet();
            targetSet.Bar.TotalWeight = targetSession.CurrentTargetWeight;
            targetSession.LiftSets.Push(targetSet);

            //create best first search for lift sessions
            BestFirstSearch<LiftSession> liftSessionSearch = new BestFirstSearch<LiftSession>(targetSession, originSession);

            //TODO: currently will spin out if no solution possible (i.e. if one of the target set weights = 300).
            //TODO: related to above - expand session method should check when it has overshot target, then not added node to heap
            //search and return a response which will contain whether a solution was found or not
            BestFirstSearch<LiftSession>.SearchResponse response = liftSessionSearch.Search(); 
        }

        //creates initial weightstacks that would exist in a "gym"
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
