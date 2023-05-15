using System.Collections.Generic;

namespace UnityFramework.Runtime
{
    public static class DictionaryPool<TKey, TValue>
    {
        static int m_Count = 8;
        static Stack<Dictionary<TKey, TValue>> m_Stack = new Stack<Dictionary<TKey, TValue>>(m_Count);

        public static Dictionary<TKey, TValue> Allocate()
        {
            if (m_Stack.Count == 0)
            {
                return new Dictionary<TKey, TValue>(m_Count);
            }

            return m_Stack.Pop();
        }

        public static void Recycle(Dictionary<TKey, TValue> _dictionary)
        {
            _dictionary.Clear();
            m_Stack.Push(_dictionary);
        }
    }

    public static class DictionaryPoolExtension 
    {
        public static void Recycle<TKey, TValue>(this Dictionary<TKey, TValue> _self)
        {
            DictionaryPool<TKey,TValue>.Recycle(_self);
        }
    }
}