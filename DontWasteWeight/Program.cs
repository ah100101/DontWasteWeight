using DontWasteWeight.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axel.Data.Structures;

namespace DontWasteWeight
{
    class Program
    {
        private static decimal[] targetSets;
        //to be replaced with a priority queue
        //private static List<LiftSession> possibleLiftSessions = new List<LiftSession>();
        private static BinaryHeap<LiftSession> possibleLiftSessions = new BinaryHeap<LiftSession>();
        //possibleLiftSessions = new List<LiftSession>();
        //need to replace this with a binary search tree/heap... ;) O_O
        private static List<LiftSession> visitedSessions = new List<LiftSession>();

        static void Main(string[] args)
        {
            List<WeightStack> startingWeightStacks = CreateGymWeightStacks();

            int numSets = 6;
            targetSets = new decimal[numSets];
            targetSets[0] = 165;
            targetSets[1] = 185;
            targetSets[2] = 205;
            targetSets[3] = 225;
            targetSets[4] = 245;
            targetSets[5] = 265;

            bool solved = false;
            
            LiftSession originSession = new LiftSession();
            LiftSession solvedSession = new LiftSession();

            originSession.BarWeight = 45;
            originSession.WeightSetMoves = 0;
            originSession.SessionWeightStacks = startingWeightStacks;
            originSession.CreateBaseSet();
            originSession.CurrentTargetIndex = 0;
            originSession.CurrentTargetWeight = targetSets[0];

            //possibleLiftSessions.Add(originSession);
            possibleLiftSessions.Insert(new LiftSession(originSession));

            while (!solved && possibleLiftSessions.Size > 0)
            {
                //in a priority queue, this needs to pop the top off the stack
                //also need to test if the currentSession is at a target session or if it is a solution session
                //this needs to run until the pq is empty or all the nodes steps are greater than the current solution session
                LiftSession currentSession = new LiftSession(possibleLiftSessions.Pop());
                //possibleLiftSessions.RemoveAt(0);

                if (currentSession != null)
                {
                    visitedSessions.Add(new LiftSession(currentSession));

                    currentSession.UpdateTargetIndex(targetSets);
                    solved = currentSession.AtFinalSet(targetSets);

                    if (!solved && currentSession.LiftSets != null)
                    {
                        ExpandSession(currentSession);
                    }
                    else if(solved)
                    {
                        solvedSession = currentSession;
                    }
                }
            }
        }

        private static void ExpandSession(LiftSession currentSession)
        {
            //for some reason all the lift sets are ending up the same...the first set is good here, but when the second one is added, the first one is being modified too
            if(currentSession.CanAddPlates())
            {
                foreach(WeightStack stack in currentSession.SessionWeightStacks)
                {
                    //make this a constructor
                    PlateSet plateSetToAdd = new PlateSet();
                    plateSetToAdd.InitializePlates(stack.Weight);

                    if(currentSession.CanAddThesePlates(plateSetToAdd))
                    {
                        LiftSession newSession = new LiftSession(currentSession);
                        newSession.AddPlates(plateSetToAdd);

                        //for some reason when we add plates here, it's overriding the visited one as well, dont create newSessions like this
                        if(!SessionVisited(newSession, visitedSessions))
                        {
                            possibleLiftSessions.Insert(newSession);
                        }
                    }
                }
            }

            if(currentSession.CanRemovePlates())
            {
                if(currentSession.LiftSets.Count > 0)
                {
                    LiftSet currentLiftSet = new LiftSet(currentSession.LiftSets.Peek());

                    if (currentLiftSet != null && currentLiftSet.CanRemovePlates())
                    {
                        int plateSetCount = currentSession.LiftSets.Peek().Bar.LoadedPlates.Count;

                        for (int i = 1; i <= plateSetCount; i++)
                        {
                            LiftSession newSession = new LiftSession(currentSession);
                            newSession.StripPlates(i);

                            if(!SessionVisited(newSession, visitedSessions))
                            {
                                possibleLiftSessions.Insert(newSession);
                            }
                        }
                    }
                }
            }
        }

        /*
         * priority queue sorting
         * check which has the fewest moves
         * check which has the highest target index
         * check which has the smallest weight pile
         * check which total is closest to the next set weight
         * 
         * if we check which ones have the smallest difference to the next set weight:
         *  this takes care of the items at the start, cause it will grab which puts most weight on first
         *  after that initial set, once the difference starts going to 0 (at target) those will populate up...
         *  but it won't distinguish which ones are at higher target states
         *  so sort next on which ones have the smallest weight pile
         *  if a pile has a high target index and a small weight pile will be prioritized same as small target index with small weight pile
         *  but this is good, cause the large weight piles will go to bottom
         *  ..those large weight piles might have a high target state though, so if they're the same send those with a high target state up
         *  so far...sort by smallest different, then by weight pile, then by target state
         *  
         *  as long as weight piles keep building, the queue will keep bubbling up the lesser target states, which may overtake at some point
         *  but say we get down to a couple that are at the final target state, with the same sized weight pile..which one gets selected?
         *  the one with the fewest moves...otherwise there are multiple solutions...
         *  
         * so we need to sort first by smallest difference, then by the weight pile, then by the target state, then by fewest moves
         * :)
         * right?..RIGHT?
         */
        

        private static bool SessionVisited(LiftSession newSession, List<LiftSession> history)
        {
            //if (history.Any(ls => ls.CurrentTargetIndex == newSession.CurrentTargetIndex
            //                && ls.WeightSetMoves == newSession.WeightSetMoves
            //                && ls.UsedPlatesCount == newSession.UsedPlatesCount
            //                && ls.LiftSets.Count == newSession.LiftSets.Count
            //                && ls.LiftSets.FirstOrDefault().Bar.TotalWeight == newSession.LiftSets.FirstOrDefault().Bar.TotalWeight))
            //    return true;

            if (history.Any(ls => ls.CurrentTargetIndex == newSession.CurrentTargetIndex
                            && ls.LiftSets.Peek().Bar.TotalWeight == newSession.LiftSets.FirstOrDefault().Bar.TotalWeight))
                return true;

            return false;
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
