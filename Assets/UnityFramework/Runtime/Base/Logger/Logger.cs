using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// This component is not fit well in XR equipments.
    /// </summary>
    [AddComponentMenu("itsxwz/Tool/Logger")]
    public class Logger : MonoBehaviour
    {
        [Tooltip("Whether remain log that recorded before.")] [SerializeField]
        bool m_RemainRecord;

        string m_LogFilePath;
        StringBuilder m_StringBuilder;
        bool m_ShowLog;

        Rect m_ScrollViewRect, m_ButtonViewRect;
        GUIStyle m_LabelStyle, m_ClearBtnStyle;
        Vector2 m_ScrollViewPos;

        Texture m_ContentBg;

        void Awake()
        {
            m_LogFilePath = string.Format("{0}/{1}_LOG.log", Application.persistentDataPath, Application.productName);
            if (!m_RemainRecord)
            {
                if (File.Exists(m_LogFilePath))
                {
                    File.Delete(m_LogFilePath);
                }

                File.Create(m_LogFilePath).Dispose();
            }

            m_ContentBg = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

            m_ScrollViewRect = new Rect(0, 0, Screen.width, Screen.height * 0.95f);
            m_LabelStyle = new GUIStyle
            {
                normal = {textColor = Color.red}, wordWrap = true, fontSize = 25
            };

            m_ClearBtnStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 25,
                fontStyle = FontStyle.Bold,
                normal = {textColor = Color.black}
            };
            m_ClearBtnStyle.normal.background = Texture2D.blackTexture;

            //Set clear button rect.
            m_ButtonViewRect = new Rect(0, Screen.height * 0.95f, Screen.width, Screen.height * 0.05f);

            m_StringBuilder = new StringBuilder();
            Application.logMessageReceived += OnLogMessageReceived;

            DontDestroyOnLoad(this);
        }

        void Update()
        {
            //[NOTE] F4 control open or close GUI log infos.
            if (Input.GetKeyDown(KeyCode.F4))
            {
                m_ShowLog = !m_ShowLog;
            }
        }

        void OnGUI()
        {
            if (m_ShowLog)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_ContentBg);

                GUILayout.BeginArea(m_ScrollViewRect);
                {
                    m_ScrollViewPos = GUILayout.BeginScrollView(m_ScrollViewPos);
                    {
                        GUILayout.Label(m_StringBuilder.ToString(), m_LabelStyle);
                        GUILayout.EndScrollView();
                    }

                    GUILayout.EndArea();
                }

                GUILayout.BeginArea(m_ButtonViewRect);
                {
                    if (GUILayout.Button(new GUIContent("Clear"), GUILayout.Height(Screen.height * 0.05f)))
                    {
                        m_StringBuilder.Clear();
                    }
                
                    GUILayout.EndArea();
                }
            }
        }

        void OnDestroy()
        {
            using (var sw = File.AppendText(m_LogFilePath))
            {
                sw.WriteLine("--------------------------------------------------------------------\n\n");
            }

            m_StringBuilder = null;
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        void OnLogMessageReceived(string _logMessage, string _stackTrace, LogType _logType)
        {
            m_StringBuilder.Append("[").Append(_logType.ToString()).Append(" ").Append(DateTime.Now.ToString("yyyy.MM.dd HH:MM:ss")).Append("]").Append("\n");
            m_StringBuilder.Append(_logMessage).Append("\n");

            foreach (var line in _stackTrace.Split('\n'))
            {
                Match m = Regex.Match(line, @"\(at (.+)\)", RegexOptions.IgnoreCase);
                if (m.Groups.Count > 1)
                {
                    if (!(line.Contains("UnityFramework") && line.Contains("Log.cs")))
                    {
                        m_StringBuilder.Append(line).Append("\n");
                        break;
                    }
                }
            }

            m_StringBuilder.Append("\n");

            using var sw = File.AppendText(m_LogFilePath);
            sw.WriteLine(m_StringBuilder.ToString());
        }
    }
}