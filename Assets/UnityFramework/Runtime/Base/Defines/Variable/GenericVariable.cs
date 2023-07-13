using System;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 泛型变量类，T可以指定为具体的类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseVariable<T> : BaseVariable
    {
        private T m_Value;

        protected BaseVariable()
        {
            m_Value = default;
        }
        
        public override Type Type => typeof(T);

        public T Value
        {
            get => m_Value;
            set => m_Value = value;
        }

        public override object GetValue()
        {
            return m_Value;
        }

        public override void SetValue(object value)
        {
            m_Value = (T)value;
        }

        public override void Clear()
        {
            m_Value = default;
        }

        /// <summary>
        /// 泛型变量为空时, ToString的值默认为 <NULL>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_Value != null ? m_Value.ToString() : "<NULL>";
        }
    }
}