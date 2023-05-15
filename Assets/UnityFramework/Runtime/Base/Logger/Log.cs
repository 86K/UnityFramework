using System.Diagnostics;

namespace UnityFramework.Runtime
{
    public static class Log
    {
        [Conditional("ENABLE_LOG_ALL")]
        [Conditional("ENABLE_LOG_INFO")]
        [Conditional("ENABLE_LOG_INFO_AND_ABOVE")]
        public static void Info(object _message)
        {
            UnityEngine.Debug.Log($"[Info] {_message}");
        }

        [Conditional("ENABLE_LOG_ALL")]
        [Conditional("ENABLE_LOG_INFO_AND_ABOVE")]
        [Conditional("ENABLE_LOG_WARN")]
        [Conditional("ENABLE_LOG_WARN_AND_ABOVE")]
        public static void Warn(object _message)
        {
            UnityEngine.Debug.Log($"[Warn] {_message}");
        }

        [Conditional("ENABLE_LOG_ALL")]
        [Conditional("ENABLE_LOG_INFO_AND_ABOVE")]
        [Conditional("ENABLE_LOG_WARN_AND_ABOVE")]
        [Conditional("ENABLE_LOG_ERROR")]
        public static void Error(object _message)
        {
            UnityEngine.Debug.Log($"[Error] {_message}");
        }
    }
}