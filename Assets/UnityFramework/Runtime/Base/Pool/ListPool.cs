using System.Collections.Generic;

namespace UnityFramework.Runtime
{
    public static class ListPool<T>
    {
        static int m_Count = 8;
        static Stack<List<T>> m_Stack = new Stack<List<T>>(m_Count);

        public static List<T> Allocate()
        {
            if (m_Stack.Count == 0)
            {
                return new List<T>(m_Count);
            }

            return m_Stack.Pop();
        }

        public static void Recycle(List<T> _list)
        {
            _list.Clear();
            m_Stack.Push(_list);
        }
    }

    public static class ListPoolExtension
    {
        public static void Recycle<T>(this List<T> _self)
        {
            ListPool<T>.Recycle(_self);
        }
    }
}