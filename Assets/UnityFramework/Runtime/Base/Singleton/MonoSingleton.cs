using UnityEngine;

namespace UnityFramework.Runtime
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType(typeof(T)) as T;

                    if (m_Instance == null)
                    {
                        m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

                        if (m_Instance == null)
                        {
                            Debug.LogError($"Create mono singleton failed, type of '{typeof(T)}'");
                        }
                    }

                    if (m_Instance != null && !m_IsInitialized)
                    {
                        m_IsInitialized = true;
                        m_Instance.Init();
                    }
                }

                return m_Instance;
            }
        }

        static bool m_IsInitialized;

        void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;
            }
            else if (m_Instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            if (!m_IsInitialized)
            {
                DontDestroyOnLoad(gameObject);
                m_IsInitialized = true;
                if (!(m_Instance is null)) m_Instance.Init();
            }
        }

        /// <summary>
        /// Use this function instead of awake.
        /// </summary>
        protected virtual void Init()
        {
        }

        void OnApplicationQuit()
        {
            m_Instance = null;
        }
    }
}