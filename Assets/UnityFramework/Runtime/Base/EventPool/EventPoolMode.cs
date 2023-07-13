using System;

namespace UnityFramework.Runtime
{
    [Flags]
    internal enum EventPoolMode : byte
    {
        /// <summary>
        /// 必须存在有且只有一个事件处理句柄
        /// </summary>
        Default = 0,
        
        /// <summary>
        /// 允许不存在事件处理句柄
        /// </summary>
        AllowNoEventHandler = 1,
        
        /// <summary>
        /// 允许存在多个事件处理句柄
        /// </summary>
        AllowMultiEventHandler = 2,
        
        /// <summary>
        /// 允许存在重复的事件处理句柄
        /// </summary>
        AllowDuplicateEventHandler = 4,
    }
}