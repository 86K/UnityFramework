using UnityEngine;

namespace UnityFramework.Runtime
{
    [AddComponentMenu("itsxwz/Tool/FpsCounter")]
    public class FpsCounter : MonoBehaviour
    {
        float m_UpdateInterval;
        int m_CurrentFps;
        int m_Frames;
        float m_Accumulator;
        float m_TimeLeft;

        void Awake()
        {
            m_UpdateInterval = 0.5f;
            m_CurrentFps = 0;
            m_Frames = 0;
            m_Accumulator = 0f;
            m_TimeLeft = 0f;

            DontDestroyOnLoad(this);
        }

        void Update()
        {
            m_Frames++;
            m_Accumulator += Time.deltaTime;
            m_TimeLeft -= Time.unscaledDeltaTime;

            if (m_TimeLeft <= 0f)
            {
                m_CurrentFps = m_Accumulator > 0f ? (int) (m_Frames / m_Accumulator) : 0;
                m_Frames = 0;
                m_Accumulator = 0f;
                m_TimeLeft += m_UpdateInterval;
            }
        }

        /// <summary>
        /// [NOTE]
        /// XR equipments cannt use this function, you need to show fps data by a text.
        /// </summary>
        void OnGUI()
        {
            GUIStyle style = new GUIStyle("Box")
            {
                fontSize = 40,
                richText = true,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.color = Color.red;
            GUILayout.Box("Fps：" + m_CurrentFps, style);
        }
    }
}