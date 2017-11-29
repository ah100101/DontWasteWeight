using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWasteWeight.Core.Data.Structures
{
    /// <summary>
    /// Min Heap structure for keeping lowest valued object at top. Class must implement IComparable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryHeap<T>
    {
        protected T[] items;

        protected int size = 0;

        protected Comparison<T> comparison;

        public BinaryHeap()
        {
            Initialize(4, null);
        }

        public BinaryHeap(Comparison<T> comparison)
        {
            Initialize(4, comparison);
        }

        public BinaryHeap(int size)
        {
            Initialize(size, null);
        }

        public BinaryHeap(int size, Comparison<T> comparison)
        {
            Initialize(size, comparison);
        }

        private void Initialize(int size, Comparison<T> comparison)
        {
            items = new T[size];
            this.comparison = comparison;
            if (this.comparison == null)
                this.comparison = new Comparison<T>(Comparer<T>.Default.Compare);
        }

        public int Size
        {
            get
            {
                return size;
            }
        }

        public void Push(T item)
        {
            if (size == items.Length)
                Resize();
            items[size] = item;
            HeapifyUp(size);
            size++;
        }

        public T Peek()
        {
            return items[0];
        }

        public T Pop()
        {
            T item = items[0];
            size--;
            items[0] = items[size];
            HeapifyDown(0);
            return item;
        }

        private void Resize()
        {
            T[] resizedData = new T[items.Length * 2];
            Array.Copy(items, 0, resizedData, 0, items.Length);
            items = resizedData;
        }

        private void HeapifyUp(int childIdx)
        {
            if (childIdx > 0)
            {
                int parentIdx = (childIdx - 1) / 2;
                if (comparison.Invoke(items[childIdx], items[parentIdx]) > 0)
                {
                    T t = items[parentIdx];
                    items[parentIdx] = items[childIdx];
                    items[childIdx] = t;
                    HeapifyUp(parentIdx);
                }
            }
        }

        private void HeapifyDown(int parentIdx)
        {
            int leftChildIdx = 2 * parentIdx + 1;
            int rightChildIdx = leftChildIdx + 1;
            int largestChildIdx = parentIdx;
            if (leftChildIdx < size && comparison.Invoke(items[leftChildIdx], items[largestChildIdx]) > 0)
            {
                largestChildIdx = leftChildIdx;
            }
            if (rightChildIdx < size && comparison.Invoke(items[rightChildIdx], items[largestChildIdx]) > 0)
            {
                largestChildIdx = rightChildIdx;
            }
            if (largestChildIdx != parentIdx)
            {
                T t = items[parentIdx];
                items[parentIdx] = items[largestChildIdx];
                items[largestChildIdx] = t;
                HeapifyDown(largestChildIdx);
            }
        }
    }
}
