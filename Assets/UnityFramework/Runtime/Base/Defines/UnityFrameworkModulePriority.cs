namespace UnityFramework.Runtime
{
    /// <summary>
    /// 优先级高的模块先轮询，并且关闭操作会后执行
    /// </summary>
    internal static class UnityFrameworkModulePriority
    {
        public static readonly int Event = 7;
    }
}