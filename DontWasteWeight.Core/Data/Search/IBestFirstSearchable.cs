using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Core.Data.Search
{
    /// <summary>
    /// Interface for node objects being searched via best first search. Must be serializable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBestFirstSearchable<T> : IComparable
    {
        /// <summary>
        /// Cost of getting from initial node to current node (n)
        /// </summary>
        /// <returns>decimal</returns>
        decimal Gn();

        /// <summary>
        /// Cost of getting from current node (n) to final node
        /// </summary>
        /// <returns>decimal</returns>
        decimal Hn();

        /// <summary>
        /// Cost of h(n) + g(n)
        /// </summary>
        /// <returns>decimal</returns>
        decimal Fn();

        /// <summary>
        /// Returns array of next possible nodes
        /// </summary>
        /// <returns>T[]</returns>
        T[] Expand();

        /// <summary>
        /// Returns if this item and target item are equivalent. Determines if already visited or at goal
        /// </summary>
        /// <param name="compareItem"></param>
        /// <returns>bool</returns>
        bool IsEquivalentNode(T compareItem);
    }
}
