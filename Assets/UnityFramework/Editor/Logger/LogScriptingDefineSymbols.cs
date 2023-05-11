namespace UnityFramework.Editor
{
    using System.Linq;
    using System.Text;
    using UnityEditor;
    
    internal static class LogScriptingDefineSymbols
    {
        const string ENABLE_LOG_ALL = "ENABLE_LOG_ALL";
        const string ENABLE_LOG_INFO = "ENABLE_LOG_INFO";
        const string ENABLE_LOG_INFO_AND_ABOVE = "ENABLE_LOG_INFO_AND_ABOVE";
        const string ENABLE_LOG_WARN = "ENABLE_LOG_WARN";
        const string ENABLE_LOG_WARN_AND_ABOVE = "ENABLE_LOG_WARN_AND_ABOVE";
        const string ENABLE_LOG_ERROR = "ENABLE_LOG_ERROR";

        static readonly string[] Symbols =
        {
            ENABLE_LOG_ALL,
            ENABLE_LOG_INFO,
            ENABLE_LOG_INFO_AND_ABOVE,
            ENABLE_LOG_WARN,
            ENABLE_LOG_WARN_AND_ABOVE,
            ENABLE_LOG_ERROR
        };

        static readonly BuildTargetGroup[] TargetPlatforms =
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.WebGL
        };

        [MenuItem("itsxwz/Log/ENABLE_LOG_ALL", false, 0)]
        public static void EnableAllLogs()
        {
            AddSymbol(ENABLE_LOG_ALL);
        }

        [MenuItem("itsxwz/Log/DISABLE_LOG_ALL", false, 0)]
        public static void DisableAllLogs()
        {
            RemoveSymbols();
        }

        [MenuItem("itsxwz/Log/ENABLE_LOG_INFO", false, 0)]
        public static void EnableLogInfo()
        {
            AddSymbol(ENABLE_LOG_INFO);
        }

        [MenuItem("itsxwz/Log/ENABLE_LOG_INFO_AND_ABOVE", false, 0)]
        public static void EnableLogInfoAndAbove()
        {
            AddSymbol(ENABLE_LOG_INFO_AND_ABOVE);
        }

        [MenuItem("itsxwz/Log/ENABLE_LOG_WARN", false, 0)]
        public static void EnableLogWarn()
        {
            AddSymbol(ENABLE_LOG_WARN);
        }

        [MenuItem("itsxwz/Log/ENABLE_LOG_WARN_AND_ABOVE", false, 0)]
        public static void EnableLogWarnAndAbove()
        {
            AddSymbol(ENABLE_LOG_WARN_AND_ABOVE);
        }

        [MenuItem("itsxwz/Log/ENABLE_LOG_ERROR", false, 0)]
        public static void EnableLogError()
        {
            AddSymbol(ENABLE_LOG_ERROR);
        }

        static void AddSymbol(string _symbol)
        {
            foreach (var targetPlatform in TargetPlatforms)
            {
                var existSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetPlatform).Split(';').ToList();

                if (!existSymbols.Contains(_symbol))
                {
                    existSymbols.Add(_symbol);
                }

                foreach (var s in Symbols)
                {
                    if (existSymbols.Contains(s))
                    {
                        if (s != _symbol)
                        {
                            existSymbols.Remove(s);
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (var s in existSymbols)
                {
                    sb.Append(s).Append(";");
                }

#if UNITY_2019_4
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, sb.ToString());
#else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, existSymbols.ToArray());
#endif
            }
        }

        static void RemoveSymbols()
        {
            foreach (var targetPlatform in TargetPlatforms)
            {
                var existSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetPlatform).Split(';').ToList();
                foreach (var symbol in Symbols)
                {
                    if (existSymbols.Contains(symbol))
                    {
                        existSymbols.Remove(symbol);
                    }
                }


                StringBuilder sb = new StringBuilder();
                foreach (var s in existSymbols)
                {
                    sb.Append(s).Append(";");
                }

#if UNITY_2019_4
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, sb.ToString());
#else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, existSymbols.ToArray());
#endif
            }
        }
    }
}