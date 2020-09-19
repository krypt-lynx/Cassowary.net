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
        public int sparceIndex = -1;
    }

    public class ClSparseRowArray : IDictionary<ClAbstractVariable, ClLinearExpression>
    {
        static int nextId = 0;

        int id = 0;

        int capacity = 1024;
        int size;
        int head;

        ClAbstractVariable[] variables;
        ClLinearExpression[] rows;

        public ClSparseRowArray()
        {
            variables = new ClAbstractVariable[capacity];
            rows = new ClLinearExpression[capacity];
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

        public ICollection<ClLinearExpression> Values
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

        public ClLinearExpression this[ClAbstractVariable key]
        {
            get
            {
                return rows[key.sparceIndex];
            }
            set
            {
                rows[key.sparceIndex] = value;
            }
        }

        public bool ContainsKey(ClAbstractVariable key)
        {
            return key.sparceIndex != -1;
        }

        public void Add(ClAbstractVariable key, ClLinearExpression value)
        {
            while (variables[head] != null)
            {
                head = (head + 1) % capacity;
            }

            key.sparceIndex = head;
            variables[head] = key;
            rows[head] = value;
            head++;
            size++;
            if (size > (capacity >> 1))
            {
                Resize();
            }
        }

        private void Resize()
        {
            var oldVars = variables;
            var oldRows = rows;
            variables = new ClAbstractVariable[capacity * 2];
            rows = new ClLinearExpression[capacity * 2];

            Array.Copy(oldVars, variables, capacity);
            Array.Copy(oldRows, rows, capacity);

            capacity <<= 1;
        }

        public bool Remove(ClAbstractVariable key)
        {
            if (variables[key.sparceIndex] == null)
            {
                return false;
            }

            variables[key.sparceIndex] = null;
            key.sparceIndex = -1;
            size--;

            return true;
        }

        public bool TryGetValue(ClAbstractVariable key, out ClLinearExpression value)
        {
            if (key.sparceIndex == -1)
            {
                value = null;
                return false;
            }
            else
            {
                value = rows[key.sparceIndex];
                return true;
            }
        }

        public void Add(KeyValuePair<ClAbstractVariable, ClLinearExpression> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            variables = new ClAbstractVariable[capacity];
            rows = new ClLinearExpression[capacity];
            size = 0;
            head = 0;
        }

        public bool Contains(KeyValuePair<ClAbstractVariable, ClLinearExpression> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<ClAbstractVariable, ClLinearExpression>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<ClAbstractVariable, ClLinearExpression> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<ClAbstractVariable, ClLinearExpression>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<KeyValuePair<ClAbstractVariable, ClLinearExpression>>,
       IDictionaryEnumerator
        {
            private IDictionary<ClAbstractVariable, ClLinearExpression> sparseArray;

            private int index;
            private KeyValuePair<ClAbstractVariable, ClLinearExpression> current;
            IEnumerator<ClAbstractVariable> keyEnumerator;

            internal Enumerator(ClSparseRowArray sparseArray)
            {
                this.sparseArray = sparseArray;
                index = -1;
                keyEnumerator = sparseArray.Keys.GetEnumerator();
                current = default(KeyValuePair<ClAbstractVariable, ClLinearExpression>);
            }

            public bool MoveNext()
            {
                var hasNext = keyEnumerator.MoveNext();
                if (hasNext)
                {
                    current = new KeyValuePair<ClAbstractVariable, ClLinearExpression>(
                        keyEnumerator.Current, sparseArray[keyEnumerator.Current]);
                }
                return hasNext;
            }

            public KeyValuePair<ClAbstractVariable, ClLinearExpression> Current
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
