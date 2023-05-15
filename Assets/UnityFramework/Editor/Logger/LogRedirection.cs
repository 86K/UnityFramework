using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityFramework.Editor
{
    public static class LogRedirection
    {
        [UnityEditor.Callbacks.OnOpenAssetAttribute(-1)]
        static bool OnOpenAsset(int _instanceID, int _line)
        {
            string path = GetDirectCallScriptPath();
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            string[] data = path.Split(':');
            string uri =
                Application.dataPath.Substring(0,
                    Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal)) + data[0];
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(uri.Replace('/', '\\'),
                Convert.ToInt32(data[1]));
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
                    if ((object)UnityEditor.EditorWindow.focusedWindow == consoleInstance)
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
    }
}