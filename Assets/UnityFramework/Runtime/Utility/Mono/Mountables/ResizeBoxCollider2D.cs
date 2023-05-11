namespace UnityFramework.Runtime
{
    using UnityEngine;

    /// <summary>
    /// Resizer of BoxCollider2D size for follow to RectTransform size.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(BoxCollider2D))]
    [AddComponentMenu("itsxwz/Mono/Resize BoxCollider2D")]
    public class ResizeBoxCollider2D : MonoBehaviour
    {
        [SerializeField] bool m_IsCalculateOnce = true;
        RectTransform m_RectTransform;
        BoxCollider2D m_BoxCollider2D;
        static Vector3[] m_Corners = new Vector3[4];

        void LateUpdate()
        {
            Reposition();
            if (m_IsCalculateOnce && Application.isPlaying)
            {
                enabled = false;
            }
        }

        void Reposition()
        {
            if ((object) m_RectTransform == null)
            {
                m_RectTransform = (RectTransform) transform;
            }

            if ((object) m_BoxCollider2D == null)
            {
                m_BoxCollider2D = GetComponent<BoxCollider2D>();
            }

            m_RectTransform.GetLocalCorners(m_Corners);
            var xMin = float.MaxValue;
            var xMax = float.MinValue;
            var yMin = float.MaxValue;
            var yMax = float.MinValue;
            for (var i = 3; i >= 0; i--)
            {
                if (xMin > m_Corners[i].x)
                {
                    xMin = m_Corners[i].x;
                }

                if (xMax < m_Corners[i].x)
                {
                    xMax = m_Corners[i].x;
                }

                if (yMin > m_Corners[i].y)
                {
                    yMin = m_Corners[i].y;
                }

                if (yMax < m_Corners[i].y)
                {
                    yMax = m_Corners[i].y;
                }
            }

            m_BoxCollider2D.size = new Vector2(xMax - xMin, yMax - yMin);
        }
    }
}