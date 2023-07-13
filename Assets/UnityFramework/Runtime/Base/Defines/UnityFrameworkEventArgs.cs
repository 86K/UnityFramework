using System;

namespace UnityFramework.Runtime
{
    public abstract class UnityFrameworkEventArgs : EventArgs, IReference
    {
        public UnityFrameworkEventArgs(){}
        
        public abstract void Clear();
    }
}