using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axel.Data.Search
{
    /// <summary>
    /// Interface for implementing node object in best first search
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBestFirstSearchable<T> : IComparable
    {
        /// <summary>
        /// Cost of getting from initial node to current node (n)
        /// </summary>
        /// <returns>decimal</returns>
        decimal gn();

        /// <summary>
        /// Cost of getting from current node (n) to final node
        /// </summary>
        /// <returns>decimal</returns>
        decimal hn();

        /// <summary>
        /// Cost of h(n) + g(n)
        /// </summary>
        /// <returns>decimal</returns>
        decimal fn();

        /// <summary>
        /// Returns array of next possible nodes
        /// </summary>
        /// <returns>T[]</returns>
        T[] Expand();

        bool SameNode(T target);

        /*
            class SearchItem : IBestFirstSearchable<SearchItem>
            {
                public decimal gn()
                {
                    return 0;
                }

                public decimal hn()
                {
                    return 0;
                }

                public decimal fn()
                {
                    return 0;
                }

                public SearchItem[] Expand()
                {
                    SearchItem[] items = new SearchItem[] { };

                    return items;
                }

            }
        */
    }
}
