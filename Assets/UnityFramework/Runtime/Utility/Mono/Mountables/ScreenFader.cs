namespace UnityFramework.Runtime
{
    using UnityEngine;

    // [NOTE] 使用GUI的方式可能存在性能消耗比常规方法要高的情况
    [AddComponentMenu("itsxwz/Tool/Screen Fader")]
    public class ScreenFader : MonoSingleton<ScreenFader>
    {
        Color m_FadeFrom;
        Color m_FadeTo;

        float m_IntervalTime;
        float m_Timer;

        System.Action m_Callback;

        void Awake()
        {
            useGUILayout = false;
        }

        void OnGUI()
        {
            if (m_IntervalTime <= 0f)
            {
                enabled = false;
                return;
            }

            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            m_Timer = Mathf.Clamp01(m_Timer + Time.deltaTime * m_IntervalTime);
            var color = Color.Lerp(m_FadeFrom, m_FadeTo, m_Timer);

            var savedColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(
                new Rect(0, 0, Screen.width, Screen.height),
                Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.color = savedColor;

            if (m_Timer >= 1f)
            {
                if (color.a <= 0f)
                {
                    enabled = false;
                }

                if (m_Callback != null)
                {
                    System.Action cb = m_Callback;
                    m_Callback = null;
                    cb();
                }
            }
        }

        /// <summary>
        /// Process fading from one color to another as fullscreen overlayed quad.
        /// 调用此方法即可，通过GUI的方式达成淡入淡出的效果
        /// </summary>
        /// <param name="_start">Source opaque status.</param>
        /// <param name="_end">Target opaque status.</param>
        /// <param name="_time">Time of fading.</param>
        /// <param name="_action">Optional callback on success ending of fading.</param>
        public void Process(Color _start, Color _end, float _time, System.Action _action = null)
        {
            m_FadeFrom = _start;
            m_FadeTo = _end;
            m_Callback = _action;
            m_Timer = 0f;
            m_IntervalTime = _time > 0f ? 1f / _time : 0f;
            enabled = m_IntervalTime > 0f;
        }
    }
}