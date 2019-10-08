using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab6
{
    public class DynamicList<T> : IEnumerable<T>
    {
        public int Count { get; private set; } = 0;

        private T[] array = { default(T) };

        public T this[int index]
        {
            get
            {
                int localIndex = TranslateIndex(index);
                if (!IsIndexReachable(localIndex))
                    throw new IndexOutOfRangeException(String.Format("Index {0} is out of range 0..{1}", index, Count));
                return array[localIndex];
            }
            set
            {
                int localIndex = TranslateIndex(index);
                if (!IsIndexReachable(localIndex))
                    throw new IndexOutOfRangeException(String.Format("Index {0} is out of range 0..{1}", index, Count));
                array[localIndex] = value;
            }
        }

        private void IncreaseSize()
        {
            Array.Resize(ref array, array.Length * 2);
        }

        private bool IsIndexReachable(int index)
        {
            return index >= 0 && index < Count;
        }

        private int TranslateIndex(int index)
        {
            return index < 0 ? Count + index : index;
        }

        public void Add(T item)
        {
            T[] a = { item };
            AddRange(ref a);
        }

        public void AddRange(ref T[] items)
        {
            while (Count + items.Length > array.Length)
                IncreaseSize();
            foreach (T item in items)
                array[Count++] = item;
        }

        public bool Remove(T item)
        {
            int index = Array.IndexOf(array, item, 0, Count);
            if (index != -1)
                RemoveAtUnsafely(index);
            return index != -1;
        }

        private void RemoveAtUnsafely(int index)
        {
            for (int i = index; i < Count - 1; i++)
                array[i] = array[i + 1];
            Count--;
        }

        public bool RemoveAt(int index)
        {
            int localIndex = TranslateIndex(index);
            if (IsIndexReachable(localIndex))
            {
                RemoveAtUnsafely(localIndex);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            Array.Clear(array, 0, Count);
            Count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DynamicListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class DynamicListEnumerator<T> : IEnumerator<T>
    {
        private T _current = default(T);
        private DynamicList<T> _list;
        private int _index = -1;

        public DynamicListEnumerator(DynamicList<T> list)
        {
            _list = list;
        }
        public T Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public void Dispose()
        {
            _current = default(T);
        }

        public bool MoveNext()
        {
            bool canMove = _index + 1 < _list.Count;
            if (canMove)
                _current = _list[++_index];
            return canMove;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
