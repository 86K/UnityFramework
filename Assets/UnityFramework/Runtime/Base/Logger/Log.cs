using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public static class Log
    {
        [Conditional("ENABLE_LOG_ALL")]
        [Conditional("ENABLE_LOG_INFO")]
        [Conditional("ENABLE_LOG_INFO_AND_ABOVE")]
        public static void Info(object _message)
        {
            UnityEngine.Debug.Log(_message);
        }

        [Conditional("ENABLE_LOG_ALL")]
        [Conditional("ENABLE_LOG_INFO_AND_ABOVE")]
        [Conditional("ENABLE_LOG_WARN")]
        [Conditional("ENABLE_LOG_WARN_AND_ABOVE")]
        public static void Warn(object _message)
        {
            UnityEngine.Debug.LogWarning(_message);
        }

        [Conditional("ENABLE_LOG_ALL")]
        [Conditional("ENABLE_LOG_INFO_AND_ABOVE")]
        [Conditional("ENABLE_LOG_WARN_AND_ABOVE")]
        [Conditional("ENABLE_LOG_ERROR")]
        public static void Error(object _message)
        {
            UnityEngine.Debug.LogError(_message);
        }

#if UNITY_EDITOR
        [UnityEditor.Callbacks.OnOpenAssetAttribute(-1)]
        static bool OnOpenAsset(int _instanceID, int _line)
        {
            string path = GetDirectCallScriptPath();
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            string[] data = path.Split(':');
            string uri = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal)) + data[0];
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(uri.Replace('/', '\\'), Convert.ToInt32(data[1]));
            return true;
        }

        static string GetDirectCallScriptPath()
        {
            var ConsoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            var fieldInfo = ConsoleWindowType.GetField("ms_ConsoleWindow",
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.NonPublic);
            if (fieldInfo != null)
            {
                var consoleInstance = fieldInfo.GetValue(null);
                if (consoleInstance != null)
                {
                    if ((object) UnityEditor.EditorWindow.focusedWindow == consoleInstance)
                    {
                        fieldInfo = ConsoleWindowType.GetField("m_ActiveText",
                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                        if (fieldInfo != null)
                        {
                            string activeText = fieldInfo.GetValue(consoleInstance).ToString();
                            string[] lines = activeText.Split('\n');
                            foreach (var line in lines)
                            {
                                Match m = Regex.Match(line, @"\(at (.+)\)", RegexOptions.IgnoreCase);
                                if (m.Groups.Count > 1)
                                {
                                    string path = m.Groups[1].Value;
                                    
                                    // Filter out framework's scripts.
                                    if (!(path.Contains("UnityFramework") && path.Contains("Log.cs")))
                                    {
                                        return path;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }
#endif
    }
}