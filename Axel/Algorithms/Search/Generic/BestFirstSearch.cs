using Axel.Data.Search;
using Axel.Data.Structures;
using Axel.Utilities;
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

        /// <summary>
        /// Boolean for storing if search is solved or not
        /// </summary>
        protected bool _solved;

        /// <summary>
        /// Contains all nodes that have been visited
        /// </summary>
        protected List<T> _history;

        protected Comparison<T> _comparison;

        #region Properties

        private Comparison<T> Comparison
        {
            get
            {
                if (_comparison != null)
                    return _comparison;
                else
                {
                    _comparison = new Comparison<T>(Comparer<T>.Default.Compare);
                    return _comparison;
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
                return _history;
            }
            set
            {
                _history = value;
            }
        }

        /// <summary>
        /// Target item search finds solution to
        /// </summary>
        public T Target
        {
            get
            {
                return _target;
            }

            set
            {
                _target = value;
            }
        }

        /// <summary>
        /// Starting item search finds solution from
        /// </summary>
        public T Origin
        {
            get
            {
                return _origin;
            }

            set
            {
                _origin = value;
            }
        }

        /// <summary>
        /// Binary heap for storing items by sort
        /// </summary>
        public BinaryHeap<T> BinaryHeap
        {
            get
            {
                return _binaryheap;
            }

            set
            {
                _binaryheap = value;
            }
        }

        /// <summary>
        /// Returns if search was solved
        /// </summary>
        public bool Solved
        {
            get
            {
                return _solved;
            }
        }

        /// <summary>
        /// Current top node of search
        /// </summary>
        public T Current
        {
            get
            {
                return _binaryheap.Peak();
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
            _target = target;
            _origin = origin;
            _solved = false;
            _binaryheap = new BinaryHeap<T>();
            _binaryheap.Insert(_origin);
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
            _target = target;
            _origin = origin;
            _solved = false;
            _binaryheap = new BinaryHeap<T>();
            _binaryheap.Insert(_origin);
        }

        /// <summary>
        /// Expands top item in binary heap and adds nodes back
        /// </summary>
        protected void Expand(T node)
        {
            //call node's expand method for possible moves
            T[] expandedNodes = node.Expand();

            //if expanded nodes returned, and history has items
            if(expandedNodes != null && expandedNodes.Count() > 0 && _history.Count > 0)
            {
                //if expandedNodes possible, determine if visited
                foreach(T expandedNode in expandedNodes)
                {
                    //if there are no equivalent nodes in history, add it to binary heap
                    if(!_history.Any<T>(item => item.IsEquivalentNode(expandedNode)))
                        _binaryheap.Insert(expandedNode);
                }
            }
        }

        /// <summary>
        /// Executes best first search
        /// </summary>
        public SearchResponse<T> Search()
        {
            //initialize response to be returned
            SearchResponse<T> response = new SearchResponse<T>();

            try
            {
                //if an origin and target node has been set, continue search
                if(_origin != null && _target != null)
                {
                    //while the search is not solved and binaryheap has items, expand best item until at goal
                    while(!_solved && _binaryheap.Size > 0)
                    {
                        //create clone of binary heap top
                        T current = Cloner.Clone<T>(_binaryheap.Pop());

                        //add node to history to prevent visiting again
                        _history.Add(current);

                        //if current and target are not the same
                        //if(Comparison.Invoke(current, _target) != 0)
                        if(_target.IsEquivalentNode(current))
                        {
                            //expand the current node, add results to heap
                            Expand(current);
                        }
                        else
                        {
                            //if they are the same then a solution was found
                            _solved = true;
                            response.Solution = current;
                            response.SolutionFound = true;
                            response.Succeeded = true;
                        }
                    }

                    if (!_solved)
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
        /// True if current node is at solution
        /// </summary>
        /// <returns>bool</returns>
        protected bool FoundSolution()
        {
            return false;
        }

        #endregion

        #region Response

        /// <summary>
        /// Response class that gets returned post best first search
        /// </summary>
        /// <typeparam name="U"></typeparam>
        public class SearchResponse<U> where U : T
        {
            private bool _succeeded;
            private bool _solutionFound;
            private U _solution;
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

            public U Solution
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
