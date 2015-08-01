using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axel.Data.Structures
{
    public class BinaryHeap<T>
    {
        protected T[] _items;

        protected int _size = 0;

        protected Comparison<T> _comparison;

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
            _items = new T[size];
            _comparison = comparison;
            if (_comparison == null)
                _comparison = new Comparison<T>(Comparer<T>.Default.Compare);
        }

        public int Size
        {
            get
            {
                return _size;
            }
        }

        public void Insert(T item)
        {
            if (_size == _items.Length)
                Resize();
            _items[_size] = item;
            HeapifyUp(_size);
            _size++;
        }

        public T Peak()
        {
            return _items[0];
        }

        public T Pop()
        {
            T item = _items[0];
            _size--;
            _items[0] = _items[_size];
            HeapifyDown(0);
            return item;
        }

        private void Resize()
        {
            T[] resizedData = new T[_items.Length * 2];
            Array.Copy(_items, 0, resizedData, 0, _items.Length);
            _items = resizedData;
        }

        private void HeapifyUp(int childIdx)
        {
            if (childIdx > 0)
            {
                int parentIdx = (childIdx - 1) / 2;
                if (_comparison.Invoke(_items[childIdx], _items[parentIdx]) > 0)
                {
                    // swap parent and child
                    T t = _items[parentIdx];
                    _items[parentIdx] = _items[childIdx];
                    _items[childIdx] = t;
                    HeapifyUp(parentIdx);
                }
            }
        }

        private void HeapifyDown(int parentIdx)
        {
            int leftChildIdx = 2 * parentIdx + 1;
            int rightChildIdx = leftChildIdx + 1;
            int largestChildIdx = parentIdx;
            if (leftChildIdx < _size && _comparison.Invoke(_items[leftChildIdx], _items[largestChildIdx]) > 0)
            {
                largestChildIdx = leftChildIdx;
            }
            if (rightChildIdx < _size && _comparison.Invoke(_items[rightChildIdx], _items[largestChildIdx]) > 0)
            {
                largestChildIdx = rightChildIdx;
            }
            if (largestChildIdx != parentIdx)
            {
                T t = _items[parentIdx];
                _items[parentIdx] = _items[largestChildIdx];
                _items[largestChildIdx] = t;
                HeapifyDown(largestChildIdx);
            }
        }
    }
}
