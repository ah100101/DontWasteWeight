using DontWasteWeight.Core.Data.Search;
using DontWasteWeight.Core.Data.Structures;
using DontWasteWeight.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Core.Algorithms.Search.Generic
{
    // Summary:
    //      Represents a variable size Best First Search (A* Search) Algorithm for finding best path to solution element
    //      Solution found by evaluating f(n) = h(n) + g(n) or (distance from start) + (est. distance to target)
    // Type parameters:
    //   T:
    //     Specifies the type of elements in the stack. Must implement IBestFirstSearchable
    public class BestFirstSearch<T> where T : IBestFirstSearchable<T>
    {
        #region Members

        /// <summary>
        /// Target item search finds solution to
        /// </summary>
        protected T target;

        /// <summary>
        /// Starting item search finds solution from
        /// </summary>
        protected T origin;

        /// <summary>
        /// Binary heap for storing items by sort
        /// </summary>
        protected BinaryHeap<T> binaryheap;

        /// <summary>
        /// Boolean for storing if search is solved or not
        /// </summary>
        protected bool solved;

        /// <summary>
        /// Contains all nodes that have been visited
        /// </summary>
        protected List<T> history;

        protected Comparison<T> comparison;

        #endregion

        #region Properties

        private Comparison<T> Comparison
        {
            get
            {
                if (comparison != null)
                    return comparison;
                else
                {
                    comparison = new Comparison<T>(Comparer<T>.Default.Compare);
                    return comparison;
                }
            }
        }

        /// <summary>
        /// Contains all nodes that have been visited
        /// </summary>
        public List<T> History
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
            }
        }

        /// <summary>
        /// Target item search finds solution to
        /// </summary>
        public T Target
        {
            get
            {
                return target;
            }

            set
            {
                target = value;
            }
        }

        /// <summary>
        /// Starting item search finds solution from
        /// </summary>
        public T Origin
        {
            get
            {
                return origin;
            }

            set
            {
                origin = value;
            }
        }

        /// <summary>
        /// Binary heap for storing items by sort
        /// </summary>
        public BinaryHeap<T> BinaryHeap
        {
            get
            {
                return binaryheap;
            }

            set
            {
                binaryheap = value;
            }
        }

        /// <summary>
        /// Returns if search was solved
        /// </summary>
        public bool Solved
        {
            get
            {
                return solved;
            }
        }

        /// <summary>
        /// Current top node of search
        /// </summary>
        public T Current
        {
            get
            {
                return binaryheap.Peek();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialized best first search with starting and ending nodes
        /// </summary>
        /// <param name="target">End node</param>
        /// <param name="origin">Starting node</param>
        public BestFirstSearch(T target, T origin)
        {
            Initialize(target, origin);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes best first search with starting and ending items
        /// </summary>
        /// <param name="target">End node</param>
        /// <param name="origin">Starting node</param>
        public void InitializeSearch(T target, T origin)
        {
            Initialize(target, origin);
        }

        /// <summary>
        /// Expands top item in binary heap and adds nodes back
        /// </summary>
        protected void Expand(T node)
        {
            //call node's expand method for possible moves
            T[] expandedNodes = node.Expand();

            //if expanded nodes returned, and history has items
            if(expandedNodes != null && expandedNodes.Count() > 0 && history.Count > 0)
            {
                //if expandedNodes possible, determine if visited
                foreach(T expandedNode in expandedNodes)
                {
                    //if there are no equivalent nodes in history, add it to binary heap
                    if(!history.Any<T>(item => item.IsEquivalentNode(expandedNode)))
                        binaryheap.Push(expandedNode);
                }
            }
        }

        /// <summary>
        /// Executes best first search
        /// </summary>
        public SearchResponse Search()
        {
            //initialize response to be returned
            SearchResponse response = new SearchResponse();

            try
            {
                //if an origin and target node has been set, continue search
                if(origin != null && target != null)
                {
                    //while the search is not solved and binaryheap has items, expand best item until at goal
                    while(!solved && binaryheap.Size > 0)
                    {
                        //create clone of binary heap top
                        T current = Cloner.Clone<T>(binaryheap.Pop());

                        //add node to history to prevent visiting again
                        history.Add(current);

                        //if current and target are not the same
                        if(!target.IsEquivalentNode(current))
                        {
                            //expand the current node, add results to heap
                            Expand(current);
                        }
                        else
                        {
                            //if they are the same then a solution was found
                            solved = true;
                            response.Solution = current;
                            response.SolutionFound = true;
                            response.Succeeded = true;
                        }
                    }

                    if (!solved)
                        response.SolutionFound = false;
                }
                else
                {
                    throw new Exception("Origin and Target nodes are not initialized");
                }
            }
            catch(Exception ex)
            {
                response.Solution = default(T);
                response.SolutionFound = false;
                response.Succeeded = false;
                response.ErrorException = ex;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Initializes member variables
        /// </summary>
        /// <param name="target">Node to search for</param>
        /// <param name="origin">Node to search from</param>
        private void Initialize(T target, T origin)
        {
            this.target = target;
            this.origin = origin;
            solved = false;
            binaryheap = new BinaryHeap<T>();
            binaryheap.Push(this.origin);
            history = new List<T>();
        }

        #endregion

        #region Response

        /// <summary>
        /// Response class that gets returned post best first search
        /// </summary>
        public class SearchResponse
        {
            private bool _succeeded;
            private bool _solutionFound;
            private T _solution;
            private string _errorMessage;
            private Exception _errorException;

            #region Properties

            public bool Succeeded
            {
                get
                {
                    return _succeeded;
                }
                set
                {
                    _succeeded = value;
                }
            }

            public bool SolutionFound
            {
                get
                {
                    return _solutionFound;
                }
                set
                {
                    _solutionFound = value;
                }
            }

            public T Solution
            {
                get
                {
                    return _solution;
                }
                set
                {
                    _solution = value;
                }
            }

            public string ErrorMessage
            {
                get
                {
                    return _errorMessage;
                }
                set
                {
                    _errorMessage = value;
                }
            }

            public Exception ErrorException
            {
                get
                {
                    return _errorException;
                }
                set
                {
                    _errorException = value;
                }
            }

            #endregion

            #region Constructors

            public SearchResponse()
            {
                _succeeded = false;
                _solutionFound = false;
            }

            #endregion

        }

        #endregion
    }
}
