namespace UnityFramework.Runtime
{
    // [NOTE] 使用此计时器系统，虽然很方便，但是在场景跳转等情况下需要额外注意计时器的销毁！！！
    public class Timer
    {
        float m_TotalCountTime;
        float m_CurCountTime;
        
        float m_TotalExcuteTime;
        float m_CurExcuteTime;
        
        int m_ExcuteCount;

        public int GetLeftTime()
        {
            return (int) m_TotalCountTime - m_ExcuteCount;
        }

        /// <summary>
        /// If countdown is overed?
        /// </summary>
        /// <param name="_deltaTime"></param>
        /// <returns></returns>
        bool Tick(float _deltaTime)
        {
            m_CurCountTime -= _deltaTime;
            //Reserve one second for the countdown to reach zero.
            return m_CurCountTime <= -1F;
        }

        /// <summary>
        /// Whether excuting callback can be excuted accroding to the delta time?
        /// </summary>
        /// <param name="_deltaTime"></param>
        /// <returns></returns>
        bool Excute(float _deltaTime)
        {
            m_CurExcuteTime += _deltaTime;
            if (m_CurExcuteTime >= m_TotalExcuteTime)
            {
                m_CurExcuteTime = 0f;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Fill time to reset timer.
        /// </summary>
        /// <param name="_addLast"></param>
        public void FillTime(bool _addLast = false)
        {
            if (_addLast)
            {
                m_CurCountTime = m_TotalCountTime;
                m_CurCountTime += m_TotalCountTime;
            }
            else
            {
                m_CurCountTime = m_TotalCountTime;
            }
        }

        public int m_Id;
        public bool m_IsActivate;
        public ExcuteType m_ExcuteType;

        public delegate void Callback();
        public Callback ExcutingCallback, EndCallback;

        public Timer(int _id, float _countTime, float _excuteTime = 1f, Callback _excutingCallback = null, Callback _endCallback = null,
            ExcuteType _excuteType = ExcuteType.Once)
        {
            m_Id = _id;
            m_TotalExcuteTime = _excuteTime;
            m_TotalCountTime = _countTime + 1;
            m_IsActivate = true;
            m_ExcuteType = _excuteType;
            ExcutingCallback = _excutingCallback;
            EndCallback = _endCallback;

            FillTime();
        }

        public void OnUpdate(float _deltaTime)
        {
            if (!m_IsActivate)
                return;

            if (Excute(_deltaTime))
            {
                ++m_ExcuteCount;
                ExcutingCallback?.Invoke();
            }

            if (Tick(_deltaTime))
            {
                EndCallback?.Invoke();

                if (m_ExcuteType == ExcuteType.Loop)
                {
                    FillTime();
                }
                else if (m_ExcuteType == ExcuteType.Once)
                {
                    m_IsActivate = false;
                }
            }
        }
    }
}