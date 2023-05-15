using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityFramework.Runtime
{
    [StructLayout(LayoutKind.Auto)]
    public struct UnityFrameworkLinkedListRange<T> : IEnumerable<T>
    {
        private readonly LinkedListNode<T> m_First;
        private readonly LinkedListNode<T> m_Last;
        
        public UnityFrameworkLinkedListRange(LinkedListNode<T> first, LinkedListNode<T> last)
        {
            if (first == null || last == null || first == last)
            {
                throw new Exception("Range is invalid.");
            }

            m_First = first;
            m_Last = last;
        }

        public bool IsValid => m_First != null && m_Last != null && m_First != m_Last;
        
        public LinkedListNode<T> First => m_First;

        public LinkedListNode<T> Last => m_Last;
        
        public int Count
        {
            get
            {
                if (!IsValid)
                {
                    return 0;
                }

                int count = 0;
                for (LinkedListNode<T> current = m_First; current != null && current != m_Last; current = current.Next)
                {
                    count++;
                }

                return count;
            }
        }
        
        public bool Contains(T value)
        {
            for (LinkedListNode<T> current = m_First; current != null && current != m_Last; current = current.Next)
            {
                if (current.Value.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }
        
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        [StructLayout(LayoutKind.Auto)]
        public struct Enumerator : IEnumerator<T>
        {
            private readonly UnityFrameworkLinkedListRange<T> m_GameFrameworkLinkedListRange;
            private LinkedListNode<T> m_Current;
            private T m_CurrentValue;

            internal Enumerator(UnityFrameworkLinkedListRange<T> range)
            {
                if (!range.IsValid)
                {
                    throw new Exception("Range is invalid.");
                }

                m_GameFrameworkLinkedListRange = range;
                m_Current = m_GameFrameworkLinkedListRange.m_First;
                m_CurrentValue = default;
            }
            
            public T Current => m_CurrentValue;

            object IEnumerator.Current => m_CurrentValue;
            
            public void Dispose()
            {
            }
            
            public bool MoveNext()
            {
                if (m_Current == null || m_Current == m_GameFrameworkLinkedListRange.m_Last)
                {
                    return false;
                }

                m_CurrentValue = m_Current.Value;
                m_Current = m_Current.Next;
                return true;
            }
            
            void IEnumerator.Reset()
            {
                m_Current = m_GameFrameworkLinkedListRange.m_First;
                m_CurrentValue = default;
            }
        }
    }
}
