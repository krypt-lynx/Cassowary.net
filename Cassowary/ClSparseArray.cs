using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cassowary
{
    public abstract partial class ClAbstractVariable
    {
        internal int SparceRowIndex = -1;
        internal int SparceColumnIndex = -1;
    }

    public class ClSparseColumnArray : ClSparseArray<HashSet<ClAbstractVariable>>
    {
        protected override int GetItemSparseIndex(ClAbstractVariable var)
        {
            return var.SparceColumnIndex;
        }

        protected override void SetItemSparseIndex(ClAbstractVariable var, int index)
        {
            var.SparceColumnIndex = index;
        }
    }

    public class ClSparseRowArray : ClSparseArray<ClLinearExpression>
    {
        protected override int GetItemSparseIndex(ClAbstractVariable var)
        {
            return var.SparceRowIndex;
        }

        protected override void SetItemSparseIndex(ClAbstractVariable var, int index)
        {
            var.SparceRowIndex = index;
        }
    }

    public abstract class ClSparseArray<T> : IDictionary<ClAbstractVariable, T>
    {
        static int nextId = 0;

        int id = 0;

        int capacity = 2048;
        int size;
        int head;

        ClAbstractVariable[] variables;
        T[] rows;

        public ClSparseArray()
        {
            variables = new ClAbstractVariable[capacity];
            rows = new T[capacity];
            size = 0;
            head = 0;
        }

        public ICollection<ClAbstractVariable> Keys
        {
            get
            {
                return variables.Where(x => x != null).ToArray();
            }
        }

        protected abstract int GetItemSparseIndex(ClAbstractVariable var);

        protected abstract void SetItemSparseIndex(ClAbstractVariable var, int index);

        public ICollection<T> Values
        {
            get
            {
                return rows.Where(x => x != null).ToArray();
            }
        }

        public int Count
        {
            get
            {
                return size;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T this[ClAbstractVariable key]
        {
            get
            {
                return rows[GetItemSparseIndex(key)];
            }
            set
            {
                rows[GetItemSparseIndex(key)] = value; // assuming variable belongs to this sparce array
            }
        }

        public bool ContainsKey(ClAbstractVariable key)
        {
            return GetItemSparseIndex(key) != -1;
        }

        public void Add(ClAbstractVariable key, T value)
        {
            while (variables[head] != null)
            {
                head = (head + 1) % capacity;
            }

            SetItemSparseIndex(key, head);
            variables[head] = key;
            rows[head] = value;
            head = (head + 1) % capacity;
            size++;

            if (size > (capacity >> 2))
            {
                Resize();
            }
        }

        private void Resize()
        {
            var oldVars = variables;
            var oldRows = rows;
            variables = new ClAbstractVariable[capacity * 2];
            rows = new T[capacity * 2];

            Array.Copy(oldVars, variables, capacity);
            Array.Copy(oldRows, rows, capacity);

            capacity <<= 1;
        }

        public bool Remove(ClAbstractVariable key)
        {
            var index = GetItemSparseIndex(key);
            if (variables[index] == null)
            {
                return false;
            }

             
            variables[index] = null;
            rows[index] = default(T);
            SetItemSparseIndex(key, -1);
            size--;

            return true;
        }

        public bool TryGetValue(ClAbstractVariable key, out T value)
        {
            var index = GetItemSparseIndex(key);
            if (index == -1)
            {
                value = default(T);
                return false;
            }
            else
            {
                value = rows[index];
                return true;
            }
        }

        public void Add(KeyValuePair<ClAbstractVariable, T> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            variables = new ClAbstractVariable[capacity];
            rows = new T[capacity];
            size = 0;
            head = 0;
        }

        public bool Contains(KeyValuePair<ClAbstractVariable, T> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<ClAbstractVariable, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<ClAbstractVariable, T> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<ClAbstractVariable, T>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<KeyValuePair<ClAbstractVariable, T>>,
       IDictionaryEnumerator
        {
            private IDictionary<ClAbstractVariable, T> sparseArray;

            private int index;
            private KeyValuePair<ClAbstractVariable, T> current;
            IEnumerator<ClAbstractVariable> keyEnumerator;

            internal Enumerator(ClSparseArray<T> sparseArray)
            {
                this.sparseArray = sparseArray;
                index = -1;
                keyEnumerator = sparseArray.Keys.GetEnumerator();
                current = default(KeyValuePair<ClAbstractVariable, T>);
            }

            public bool MoveNext()
            {
                var hasNext = keyEnumerator.MoveNext();
                if (hasNext)
                {
                    current = new KeyValuePair<ClAbstractVariable, T>(
                        keyEnumerator.Current, sparseArray[keyEnumerator.Current]);
                }
                return hasNext;
            }

            public KeyValuePair<ClAbstractVariable, T> Current
            {
                get { return current; }
            }

            public void Dispose()
            {
                keyEnumerator.Dispose();
            }

            object IEnumerator.Current
            {
                get
                {
                    return current;
                }
            }

            void IEnumerator.Reset()
            {
                keyEnumerator.Reset();
            }

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    return new DictionaryEntry(current.Key, current.Value);
                }
            }

            object IDictionaryEnumerator.Key
            {
                get
                {
                    return current.Key;
                }
            }

            object IDictionaryEnumerator.Value
            {
                get
                {
                    return current.Value;
                }
            }
        }
    }
}
