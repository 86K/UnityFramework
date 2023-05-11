using System;

namespace UnityFramework.Runtime
{
    public class ObjectPool<T> where T : new()
    {
        static int m_GrowSize = 20;
        static T[] m_Pool;
        static int m_NextIndex = 0;
        
        public int Capacity
        {
            get { return m_Pool.Length; }
        }
        
        public int Available
        {
            get { return m_Pool.Length - m_NextIndex; }
        }

        public int Allocated
        {
            get { return m_NextIndex; }
        }

        public static T Allocate()
        {
            T t = default(T);

            if (m_Pool == null)
            {
                Resize(m_GrowSize, false);
            }

            if (m_Pool != null && m_NextIndex >= m_Pool.Length)
            {
                if (m_GrowSize > 0)
                {
                    Resize(m_Pool.Length + m_GrowSize, true);
                }
                else
                {
                    return t;
                }
            }

            if (m_Pool != null && m_NextIndex >= 0 && m_NextIndex < m_Pool.Length)
            {
                t = m_Pool[m_NextIndex];
                m_NextIndex++;
            }

            return t;
        }

        public static void Recycle(T _t)
        {
            if (_t == null)
            {
                return;
            }

            if (m_NextIndex > 0)
            {
                m_NextIndex--;
                m_Pool[m_NextIndex] = _t;
            }
        }

        public static void Reset()
        {
            int len = m_GrowSize;
            if (m_Pool != null)
            {
                len = m_Pool.Length;
            }

            Resize(len, false);
            m_NextIndex = 0;
        }

        static void Resize(int _size, bool _copyExisting)
        {
            int count = 0;

            T[] newPool = new T[_size];

            if (m_Pool != null && _copyExisting)
            {
                count = m_Pool.Length;
                Array.Copy(m_Pool, newPool, Math.Min(count, _size));
            }

            for (int i = count; i < _size; i++)
            {
                newPool[i] = new T();
            }

            m_Pool = newPool;
        }
    }
}