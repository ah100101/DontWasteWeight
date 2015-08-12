using Axel.Data.Search;
using Axel.Data.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axel.Algorithms.Search.Generic
{
    // Summary:
    //      Represents a variable size Best First Search (A* Search) Algorithm for finding best path to solution element
    //
    // Type parameters:
    //   T:
    //     Specifies the type of elements in the stack. Must implement IBestFirstSearchable
    public class BestFirstSearch<T> where T : IBestFirstSearchable<T>
    {
        /// <summary>
        /// Target item search finds solution to
        /// </summary>
        protected T _target;

        /// <summary>
        /// Starting item search finds solution from
        /// </summary>
        protected T _origin;

        /// <summary>
        /// Binary heap for storing items by sort
        /// </summary>
        protected BinaryHeap<T> _binaryheap;

        bool solved;

        protected void ExpandCurrentNode()
        {

        }

        protected void Search()
        {

        }

        /// <summary>
        /// True if current node is at solution
        /// </summary>
        /// <returns>bool</returns>
        protected bool FoundSolution()
        {
            return false;
        }
    }
}
