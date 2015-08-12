using DontWasteWeight.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axel.Data.Structures;
using Axel.Data.Search;

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
            originSession.UpdateTargetIndex(targetSets);
            originSession.Targets = targetSets;

            possibleLiftSessions.Insert(new LiftSession(originSession));

            while (!solved && possibleLiftSessions.Size > 0)
            {
                //in a priority queue, this needs to pop the top off the stack
                //also need to test if the currentSession is at a target session or if it is a solution session
                //this needs to run until the pq is empty or all the nodes steps are greater than the current solution session
                LiftSession currentSession = new LiftSession(possibleLiftSessions.Pop());

                if (currentSession != null)
                {
                    visitedSessions.Add(new LiftSession(currentSession));

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
            if(currentSession.CanAddPlates())
            {
                foreach(WeightStack stack in currentSession.SessionWeightStacks)
                {
                    PlateSet plateSetToAdd = new PlateSet();
                    plateSetToAdd.InitializePlates(stack.Weight);

                    if(currentSession.CanAddThesePlates(plateSetToAdd))
                    {
                        LiftSession newSession = new LiftSession(currentSession);
                        newSession.AddPlates(plateSetToAdd);

                        //update target index
                        newSession.UpdateTargetIndex(targetSets);

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

                            //update target index
                            newSession.UpdateTargetIndex(targetSets);

                            if(!SessionVisited(newSession, visitedSessions))
                            {
                                possibleLiftSessions.Insert(newSession);
                            }
                        }
                    }
                }
            }
        }

        private static bool SessionVisited(LiftSession newSession, List<LiftSession> history)
        {
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
