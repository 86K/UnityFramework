using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityFramework.Runtime
{
    public sealed class UnityFrameworkLinkedList<T> : ICollection<T>, ICollection
    {
        private readonly LinkedList<T> m_LinkedList;
        private readonly Queue<LinkedListNode<T>> m_CachedNodes;

        /// <summary>
        /// 链表中实际节点数量
        /// </summary>
        public int Count => m_LinkedList.Count;

        /// <summary>
        /// 链表中缓存节点数量
        /// </summary>
        public int CachedCount => m_CachedNodes.Count;

        public LinkedListNode<T> First => m_LinkedList.First;

        public LinkedListNode<T> Last => m_LinkedList.Last;

        public bool IsReadOnly => ((ICollection<T>)m_LinkedList).IsReadOnly;

        /// <summary>
        /// 同步对ICollection访问的对象
        /// </summary>
        public object SyncRoot => ((ICollection)m_LinkedList).SyncRoot;

        /// <summary>
        /// 值是否同步对ICollection的线程安全访问
        /// </summary>
        public bool IsSynchronized => ((ICollection)m_LinkedList).IsSynchronized;

        public UnityFrameworkLinkedList()
        {
            m_LinkedList = new LinkedList<T>();
            m_CachedNodes = new Queue<LinkedListNode<T>>();
        }

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddAfter(node, newNode);
            return newNode;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            m_LinkedList.AddAfter(node, newNode);
        }
        
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddBefore(node, newNode);
            return newNode;
        }
        
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            m_LinkedList.AddBefore(node, newNode);
        }
        
        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddFirst(node);
            return node;
        }
        
        public void AddFirst(LinkedListNode<T> node)
        {
            m_LinkedList.AddFirst(node);
        }
        
        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddLast(node);
            return node;
        }
        
        public void AddLast(LinkedListNode<T> node)
        {
            m_LinkedList.AddLast(node);
        }
        
        public bool Contains(T value)
        {
            return m_LinkedList.Contains(value);
        }
        
        public void CopyTo(T[] array, int index)
        {
            m_LinkedList.CopyTo(array, index);
        }
        
        public void CopyTo(Array array, int index)
        {
            ((ICollection)m_LinkedList).CopyTo(array, index);
        }
        
        public LinkedListNode<T> Find(T value)
        {
            return m_LinkedList.Find(value);
        }
        
        public LinkedListNode<T> FindLast(T value)
        {
            return m_LinkedList.FindLast(value);
        }
        
        public bool Remove(T value)
        {
            LinkedListNode<T> node = m_LinkedList.Find(value);
            if (node != null)
            {
                m_LinkedList.Remove(node);
                ReleaseNode(node);
                return true;
            }

            return false;
        }
        
        public void Remove(LinkedListNode<T> node)
        {
            m_LinkedList.Remove(node);
            ReleaseNode(node);
        }
        
        public void RemoveFirst()
        {
            LinkedListNode<T> first = m_LinkedList.First;
            if (first == null)
            {
                throw new Exception("First is invalid.");
            }

            m_LinkedList.RemoveFirst();
            ReleaseNode(first);
        }
        
        public void RemoveLast()
        {
            LinkedListNode<T> last = m_LinkedList.Last;
            if (last == null)
            {
                throw new Exception("Last is invalid.");
            }

            m_LinkedList.RemoveLast();
            ReleaseNode(last);
        }
        
        private void ReleaseNode(LinkedListNode<T> node)
        {
            node.Value = default(T);
            m_CachedNodes.Enqueue(node);
        }
        
        private LinkedListNode<T>  AcquireNode(T value)
        {
            LinkedListNode<T> node = null;
            if (m_CachedNodes.Count > 0)
            {
                node = m_CachedNodes.Dequeue();
                node.Value = value;
            }
            else
            {
                node = new LinkedListNode<T>(value);
            }

            return node;
        }
        
        public void Clear()
        {
            LinkedListNode<T> current = m_LinkedList.First;
            while (current != null)
            {
                ReleaseNode(current);
                current = current.Next;
            }

            m_LinkedList.Clear();
        }
        
        public void ClearCachedNodes()
        {
            m_CachedNodes.Clear();
        }
        
        void ICollection<T>.Add(T value)
        {
            AddLast(value);
        }
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_LinkedList);
        }
        
        [StructLayout(LayoutKind.Auto)]
        public struct Enumerator : IEnumerator<T>
        {
            private LinkedList<T>.Enumerator m_Enumerator;

            internal Enumerator(LinkedList<T> linkedList)
            {
                if (linkedList == null)
                {
                    throw new Exception("Linked list is invalid.");
                }

                m_Enumerator = linkedList.GetEnumerator();
            }
            
            public T Current => m_Enumerator.Current;
            
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
                ((IEnumerator<T>)m_Enumerator).Reset();
            }
        }
    }
}