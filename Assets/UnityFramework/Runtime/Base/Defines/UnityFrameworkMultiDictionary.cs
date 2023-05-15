using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityFramework.Runtime
{
    public sealed class UnityFrameworkMultiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, UnityFrameworkLinkedListRange<TValue>>>
    {
        private readonly UnityFrameworkLinkedList<TValue> m_LinkedList;
        private readonly Dictionary<TKey, UnityFrameworkLinkedListRange<TValue>> m_Dictionary;
        
        public UnityFrameworkMultiDictionary()
        {
            m_LinkedList = new UnityFrameworkLinkedList<TValue>();
            m_Dictionary = new Dictionary<TKey, UnityFrameworkLinkedListRange<TValue>>();
        }
        
        public int Count => m_Dictionary.Count;
        
        public UnityFrameworkLinkedListRange<TValue> this[TKey key]
        {
            get
            {
                UnityFrameworkLinkedListRange<TValue> range = default(UnityFrameworkLinkedListRange<TValue>);
                m_Dictionary.TryGetValue(key, out range);
                return range;
            }
        }
        
        public void Clear()
        {
            m_Dictionary.Clear();
            m_LinkedList.Clear();
        }
        
        public bool Contains(TKey key)
        {
            return m_Dictionary.ContainsKey(key);
        }
        
        public bool Contains(TKey key, TValue value)
        {
            UnityFrameworkLinkedListRange<TValue> range = default(UnityFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                return range.Contains(value);
            }

            return false;
        }
        
        public bool TryGetValue(TKey key, out UnityFrameworkLinkedListRange<TValue> range)
        {
            return m_Dictionary.TryGetValue(key, out range);
        }
        
        public void Add(TKey key, TValue value)
        {
            UnityFrameworkLinkedListRange<TValue> range = default(UnityFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                m_LinkedList.AddBefore(range.Last, value);
            }
            else
            {
                LinkedListNode<TValue> first = m_LinkedList.AddLast(value);
                LinkedListNode<TValue> terminal = m_LinkedList.AddLast(default(TValue));
                m_Dictionary.Add(key, new UnityFrameworkLinkedListRange<TValue>(first, terminal));
            }
        }
        
        public bool Remove(TKey key, TValue value)
        {
            UnityFrameworkLinkedListRange<TValue> range = default(UnityFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                for (LinkedListNode<TValue> current = range.First; current != null && current != range.Last; current = current.Next)
                {
                    if (current.Value.Equals(value))
                    {
                        if (current == range.First)
                        {
                            LinkedListNode<TValue> next = current.Next;
                            if (next == range.Last)
                            {
                                m_LinkedList.Remove(next);
                                m_Dictionary.Remove(key);
                            }
                            else
                            {
                                m_Dictionary[key] = new UnityFrameworkLinkedListRange<TValue>(next, range.Last);
                            }
                        }

                        m_LinkedList.Remove(current);
                        return true;
                    }
                }
            }

            return false;
        }
        
        public bool RemoveAll(TKey key)
        {
            UnityFrameworkLinkedListRange<TValue> range = default(UnityFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                m_Dictionary.Remove(key);

                LinkedListNode<TValue> current = range.First;
                while (current != null)
                {
                    LinkedListNode<TValue> next = current != range.Last ? current.Next : null;
                    m_LinkedList.Remove(current);
                    current = next;
                }

                return true;
            }

            return false;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_Dictionary);
        }

        IEnumerator<KeyValuePair<TKey, UnityFrameworkLinkedListRange<TValue>>> IEnumerable<KeyValuePair<TKey, UnityFrameworkLinkedListRange<TValue>>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [StructLayout(LayoutKind.Auto)]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, UnityFrameworkLinkedListRange<TValue>>>
        {
            private Dictionary<TKey, UnityFrameworkLinkedListRange<TValue>>.Enumerator m_Enumerator;

            internal Enumerator(Dictionary<TKey, UnityFrameworkLinkedListRange<TValue>> dictionary)
            {
                if (dictionary == null)
                {
                    throw new Exception("Dictionary is invalid.");
                }

                m_Enumerator = dictionary.GetEnumerator();
            }

            public KeyValuePair<TKey, UnityFrameworkLinkedListRange<TValue>> Current => m_Enumerator.Current;
            
            object IEnumerator.Current => m_Enumerator.Current;
            
            public void Dispose()
            {
                m_Enumerator.Dispose();
            }

            public bool MoveNext()
            {
                return m_Enumerator.MoveNext();
            }

            void IEnumerator.Reset()
            {
                ((IEnumerator<KeyValuePair<TKey, UnityFrameworkLinkedListRange<TValue>>>)m_Enumerator).Reset();
            }
        }
    }
}
