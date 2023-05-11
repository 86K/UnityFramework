namespace UnityFramework.Runtime
{
    using UnityEngine;
    
    /// <summary>
    /// 公告板：使挂载此脚本的UI在相机视角范围内总是朝向相机
    /// </summary>
    [AddComponentMenu("itsxwz/Mono/Billboard")]
    public class Billboard : MonoBehaviour
    {
        [SerializeField] Camera m_Camera;
        [SerializeField] bool m_IsRevert;

        void Start()
        {
            if (m_Camera == null)
            {
                m_Camera = Camera.main;
            }
        }

        void Update()
        {
            if (m_Camera)
            {
                Vector3 headPosition = m_Camera.transform.position;
                Vector3 vector = headPosition - transform.position;
                vector.y = 0;
                if (vector.magnitude >= 0.5f)
                {
                    transform.rotation = Quaternion.LookRotation((m_IsRevert ? -1 : 1) * vector);
                }
            }
        }
    }
}