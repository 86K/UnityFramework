using UnityEngine;

namespace UnityFramework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("itsxwz/UnityFramework/ReferencePool")]
    public sealed class ReferencePoolComponent : MonoSingleton<ReferencePoolComponent>
    {
        [SerializeField] private ReferenceStrictCheckType m_EnableStrictCheck = ReferenceStrictCheckType.AlwaysEnable;
        
        /// <summary>
        /// 获取或设置是否开启强制检查。
        /// </summary>
        public bool EnableStrictCheck
        {
            get => ReferencePool.EnableStrictCheck;
            set
            {
                ReferencePool.EnableStrictCheck = value;
                if (value)
                {
                    Log.Info(
                        "Strict checking is enabled for the Reference Pool. It will drastically affect the performance.");
                }
            }
        }

        private void Start()
        {
            switch (m_EnableStrictCheck)
            {
                case ReferenceStrictCheckType.AlwaysEnable:
                    EnableStrictCheck = true;
                    break;

                case ReferenceStrictCheckType.OnlyEnableInDevelopment:
                    EnableStrictCheck = Debug.isDebugBuild;
                    break;

                case ReferenceStrictCheckType.OnlyEnableInEditor:
                    EnableStrictCheck = Application.isEditor;
                    break;

                case ReferenceStrictCheckType.AlwaysDisable:
                    EnableStrictCheck = false;
                    break;
            }
        }
    }
}