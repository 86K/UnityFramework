using System;

namespace UnityFramework.Runtime
{
    public abstract class BaseVariable : IReference
    {
        protected BaseVariable(){}
        
        public abstract Type Type { get; }

        public abstract object GetValue();

        public abstract void SetValue(object value);

        public abstract void Clear();
    }
}