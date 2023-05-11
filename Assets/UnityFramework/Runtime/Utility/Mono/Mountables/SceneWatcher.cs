namespace UnityFramework
{
    using UnityEngine;

    /// <summary>
    /// 通过按键驱动，控制物体的移动
    /// </summary>
    [AddComponentMenu("itsxwz/Tool/Scene watcher")]
    public class SceneWatcher : MonoBehaviour
    {
        [SerializeField] private GameObject m_Target;
        [SerializeField] private float m_Speed = 5f;
        
        void Update()
        {
            MoveByKeyBoard();
        }

        void MoveByKeyBoard()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                m_Target.transform.localPosition += m_Target.transform.TransformDirection(Vector3.forward) * m_Speed;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                m_Target.transform.localPosition += m_Target.transform.TransformDirection(Vector3.back) * m_Speed;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                m_Target.transform.localPosition += m_Target.transform.TransformDirection(Vector3.left) * m_Speed;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                m_Target.transform.localPosition += m_Target.transform.TransformDirection(Vector3.right) * m_Speed;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                m_Target.transform.localPosition += m_Target.transform.TransformDirection(Vector3.up) * m_Speed;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                m_Target.transform.localPosition += m_Target.transform.TransformDirection(Vector3.down) * m_Speed;
            }
        }
    }
}