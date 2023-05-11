namespace UnityFramework.Runtime
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Slide game object from start position to end position.
    /// </summary>
    [AddComponentMenu("itsxwz/Tool/Slide 2 other position")]
    public class SlidingObject : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        // 可以不通过按钮触发，直接绑定function到触发的代码处
        [SerializeField] private Button m_ToEndPositionButton;
        [SerializeField] private Button m_ToStartPositionButton;
        Vector3 m_StartPosition;
        [SerializeField] private Vector3 m_EndPosition;

        [Space] [Tooltip("立刻执行则不会有平滑滑动的过程")] [SerializeField]
        private bool m_ExcuteImmediately;

        [SerializeField] private float m_Duration;

        void Awake()
        {
            if (m_Target != null)
                m_StartPosition = m_Target.localPosition;
            
            OnClickToEndPositionBtn();
            OnClickToStartPositionBtn();
        }

        private void OnClickToEndPositionBtn()
        {
            if (m_ToEndPositionButton != null)
            {
                m_ToEndPositionButton.onClick.AddListener(delegate
                {
                    m_ToEndPositionButton.gameObject.SetActive(false);
                    m_ToStartPositionButton.gameObject.SetActive(true);

                    m_Target.localPosition = m_ExcuteImmediately
                        ? m_EndPosition
                        : Vector3.Lerp(m_StartPosition, m_EndPosition, m_Duration);
                });
            }
        }     
        
        private void OnClickToStartPositionBtn()
        {
            if (m_ToStartPositionButton != null)
            {
                m_ToStartPositionButton.onClick.AddListener(delegate
                {
                    m_ToEndPositionButton.gameObject.SetActive(true);
                    m_ToStartPositionButton.gameObject.SetActive(false);

                    m_Target.localPosition = m_ExcuteImmediately
                        ? m_StartPosition
                        : Vector3.Lerp(m_EndPosition, m_StartPosition, m_Duration);
                });
            }
        }
    }
}