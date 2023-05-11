using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// Control camera's rotate and view field.
    /// </summary>
    [AddComponentMenu("itsxwz/Mono/CameraController")]
    public class CameraController : MonoBehaviour
    {
        // The GameObject that camera lookat.
        public Transform m_Target;
        public float m_XSpeed = 200;
        public float m_YSpeed = 200;
        public float m_MSpeed = 10;
        public float m_YAngleMinLimit = -50;
        public float m_YAngleMaxLimit = 50;
        public float m_Distance = 10;
        public float m_MinDistance = 2;
        public float m_MaxDistance = 30;
        [Tooltip("是否需要滑动过渡")]
        public bool m_NeedDamping = true;
        float m_Damping = 5.0f;
        public float m_AngleX = 0.0f;
        public float m_AngleY = 0.0f;

        void Awake()
        {
            Vector3 angles = transform.eulerAngles;
            m_AngleX = angles.y;
            m_AngleY = angles.x;
        }

        void LateUpdate()
        {
            if (m_Target)
            {
                if (Input.GetMouseButton(1))
                {
                    m_AngleX += Input.GetAxis("Mouse X") * m_XSpeed * 0.02f;
                    m_AngleY -= Input.GetAxis("Mouse Y") * m_YSpeed * 0.02f;
                    m_AngleY = ClampAngle(m_AngleY, m_YAngleMinLimit, m_YAngleMaxLimit);
                }

                m_Distance -= Input.GetAxis("Mouse ScrollWheel") * m_MSpeed;
                m_Distance = Mathf.Clamp(m_Distance, m_MinDistance, m_MaxDistance);
                Quaternion rotation = Quaternion.Euler(m_AngleY, m_AngleX, 0.0f);
                Vector3 disVector = new Vector3(0.0f, 0.0f, -m_Distance);
                Vector3 position = rotation * disVector + m_Target.position;
                
                if (m_NeedDamping)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_Damping);
                    transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * m_Damping);
                }
                else
                {
                    transform.rotation = rotation;
                    transform.position = position;
                }
            }
        }

        float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}