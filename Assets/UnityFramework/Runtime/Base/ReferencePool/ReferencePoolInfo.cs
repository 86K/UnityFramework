using System;
using System.Runtime.InteropServices;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 引用池信息
    /// 我们不要UGF的Debugger组件，所以仅使用在编辑面板查看引用情况
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ReferencePoolInfo
    {
        private readonly Type m_Type;
        private readonly int m_UnusedReferenceCount;
        private readonly int m_UsingReferenceCount;
        private readonly int m_AcquireReferenceCount;
        private readonly int m_ReleaseReferenceCount;
        private readonly int m_AddReferenceCount;
        private readonly int m_RemoveReferenceCount;
        
        public ReferencePoolInfo(Type type, int unusedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
        {
            m_Type = type;
            m_UnusedReferenceCount = unusedReferenceCount;
            m_UsingReferenceCount = usingReferenceCount;
            m_AcquireReferenceCount = acquireReferenceCount;
            m_ReleaseReferenceCount = releaseReferenceCount;
            m_AddReferenceCount = addReferenceCount;
            m_RemoveReferenceCount = removeReferenceCount;
        }
        
        public Type Type => m_Type;

        public int UnusedReferenceCount => m_UnusedReferenceCount;
        
        public int UsingReferenceCount => m_UsingReferenceCount;
        
        public int AcquireReferenceCount => m_AcquireReferenceCount;
        
        public int ReleaseReferenceCount => m_ReleaseReferenceCount;

        public int AddReferenceCount => m_AddReferenceCount;
        
        public int RemoveReferenceCount => m_RemoveReferenceCount;
    }
}
