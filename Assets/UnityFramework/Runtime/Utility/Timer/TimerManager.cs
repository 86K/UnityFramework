namespace UnityFramework.Runtime
{
    using System.Collections.Generic;
    using UnityEngine;

    public enum ExcuteType
    {
        Once,
        Loop
    }

    /// <summary>
    /// 先声明Timer，再注册到管理器
    /// </summary>
    public class TimerManager : MonoSingleton<TimerManager>
    {
        List<Timer> m_Timers = new List<Timer>();

        public int Add(Timer _timer)
        {
            m_Timers.Add(_timer);
            return _timer.m_Id;
        }

        public void Stop(int _id)
        {
            for (int i = 0; i < m_Timers.Count; i++)
            {
                if (m_Timers[i].m_Id == _id)
                {
                    m_Timers[i].m_IsActivate = false;
                    return;
                }
            }
        }

        public void StopAll()
        {
            for (int i = 0; i < m_Timers.Count; i++)
            {
                m_Timers[i].m_IsActivate = false;
            }
        }

        void Update()
        {
            for (int i = 0; i < m_Timers.Count;)
            {
                m_Timers[i].OnUpdate(Time.deltaTime);

                if (m_Timers[i].m_IsActivate)
                {
                    i++;
                }
                else
                {
                    m_Timers[i] = null;
                    m_Timers.RemoveAt(i);
                }
            }
        }

        void OnDestroy()
        {
            StopAll();
            m_Timers.Clear();
            m_Timers = null;
        }
    }
}