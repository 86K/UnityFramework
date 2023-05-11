namespace UnityFramework.Runtime
{
    public abstract class Singleton<T> where T : new()
    {
        static T m_Instance;
        static readonly object Locker = new object();

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (Locker)
                    {
                        m_Instance = new T();
                    }
                }

                return m_Instance;
            }
        }
    }
}