using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 解决嵌套使用ScrollRect时的Drag冲突问题。
    /// 请将该脚本放置到内层ScrollRect上(外层的ScrollRect的Drag事件会被内层的拦截)
    /// </summary>
    [AddComponentMenu("itsxwz/Mono/NestedScrollRect")]
    public class NestedScrollRect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// 外层被拦截需要正常拖动的ScrollRect，可不指定，默认在父对象中找
        /// </summary>
        [SerializeField] private ScrollRect m_AnotherScrollRect;

        /// <summary>
        /// 当前的ScrollRect（本脚本所放置的物体上）的拖动方向默认为上下拖动，否则为左右拖动型
        /// </summary>
        [SerializeField] private bool m_IsUpAndDown = true;

        ScrollRect m_ThisScrollRect;

        void Awake()
        {
            m_ThisScrollRect = GetComponent<ScrollRect>();
            if (m_AnotherScrollRect == null)
                m_AnotherScrollRect = GetComponentsInParent<ScrollRect>()[1];
        }

        public void OnBeginDrag(PointerEventData _eventData)
        {
            m_AnotherScrollRect.OnBeginDrag(_eventData);
        }

        public void OnDrag(PointerEventData _eventData)
        {
            m_AnotherScrollRect.OnDrag(_eventData);
            float angle = Vector2.Angle(_eventData.delta, Vector2.up);
            //判断拖动方向，防止水平与垂直方向同时响应导致的拖动时整个界面都会动
            if (angle > 45f && angle < 135f)
            {
                m_ThisScrollRect.enabled = !m_IsUpAndDown;
                m_AnotherScrollRect.enabled = m_IsUpAndDown;
            }
            else
            {
                m_AnotherScrollRect.enabled = !m_IsUpAndDown;
                m_ThisScrollRect.enabled = m_IsUpAndDown;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_AnotherScrollRect.OnEndDrag(eventData);
            m_AnotherScrollRect.enabled = true;
            m_ThisScrollRect.enabled = true;
        }
    }
}