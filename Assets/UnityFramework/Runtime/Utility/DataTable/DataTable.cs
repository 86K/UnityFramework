using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityFramework.Runtime
{
    public abstract class IDataTable<TData> : IEnumerable<TData>, IDisposable
    {
        public abstract void Add(TData _data);
        public abstract void Remove(TData _data);
        public abstract void Clear();
        
        public IEnumerator<TData> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract void Dispose();
    }
    
    /// <summary>
    /// 数据表
    /// </summary>
    /// <typeparam name="TKey">键值段类型</typeparam>
    /// <typeparam name="TData">数据类</typeparam>
    public class DataTable<TKey, TData> : IDataTable<TData>
    {
        Dictionary<TKey, List<TData>> m_Dictionary = new Dictionary<TKey, List<TData>>();
        Func<TData, TKey> m_Key;

        /// <summary>
        /// 指定数据表的键值段
        /// </summary>
        /// <param name="_key"></param>
        public DataTable(Func<TData, TKey> _key)
        {
            m_Key = _key;
        }

        public IDictionary<TKey, List<TData>> Dictionary
        {
            get { return m_Dictionary; }
        }

        public override void Add(TData _data)
        {
            TKey key = m_Key(_data);
            if (m_Dictionary.ContainsKey(key))
            {
                m_Dictionary[key].Add(_data);
            }
            else
            {
                List<TData> datas = ListPool<TData>.Allocate();
                datas.Add(_data);
                m_Dictionary.Add(key, datas);
            }
        }

        public override void Remove(TData _data)
        {
            TKey key = m_Key(_data);
            if (m_Dictionary.ContainsKey(key))
            {
                m_Dictionary[key].Remove(_data);
            }
        }
        
        /// <summary>
        /// 获取与指定键值段属性值相等的数据
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public IEnumerable<TData> GetData(TKey _key)
        {
            List<TData> datas = null;
            if (m_Dictionary.TryGetValue(_key, out datas))
            {
                return datas;
            }

            return Enumerable.Empty<TData>();
        }

        /// <summary>
        /// 清空数据，但是保留数据结构
        /// </summary>
        public override void Clear()
        {
            foreach (var list in m_Dictionary.Values)
            {
                list.Clear();
            }

            m_Dictionary.Clear();
        }

        /// <summary>
        /// 丢弃此数据表，回收数据结构到池
        /// </summary>
        public override void Dispose()
        {
            foreach (var list in m_Dictionary.Values)
            {
                list.Recycle();
            }

            m_Dictionary.Recycle();
            m_Dictionary = null;
        }
    }
}