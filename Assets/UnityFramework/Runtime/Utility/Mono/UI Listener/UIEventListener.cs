namespace UnityFramework.Runtime
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    
    public class UIEventListener : EventTrigger
    {
        public delegate void BaseEventDelegate(GameObject _gameObject);

        public BaseEventDelegate m_OnPointerEnter;
        public BaseEventDelegate m_OnPointerExit;
        public BaseEventDelegate m_OnPointerDown;
        public BaseEventDelegate m_OnPointerUp;
        public BaseEventDelegate m_OnPointerClick;
        public BaseEventDelegate m_OnDrag;
        public BaseEventDelegate m_OnDrop;
        public BaseEventDelegate m_OnScroll;
        public BaseEventDelegate m_OnUpdateSelected;
        public BaseEventDelegate m_OnSelect;
        public BaseEventDelegate m_OnDeselect;
        public BaseEventDelegate m_OnMove;
        public BaseEventDelegate m_OnInitializePotentialDrag;
        public BaseEventDelegate m_OnBeginDrag;
        public BaseEventDelegate m_OnEndDrag;
        public BaseEventDelegate m_OnSubmit;
        public BaseEventDelegate m_OnCancel;

        public static UIEventListener Get(GameObject _gameObject)
        {
            UIEventListener listener = _gameObject.GetComponent<UIEventListener>();
            if (listener == null)
            {
                listener = _gameObject.AddComponent<UIEventListener>();
            }

            return listener;
        }

        public override void OnPointerEnter(PointerEventData _eventData)
        {
            m_OnPointerEnter?.Invoke(gameObject);
        }

        public override void OnPointerExit(PointerEventData _eventData)
        {
            m_OnPointerExit?.Invoke(gameObject);
        }

        public override void OnPointerDown(PointerEventData _eventData)
        {
            m_OnPointerDown?.Invoke(gameObject);
        }

        public override void OnPointerUp(PointerEventData _eventData)
        {
            m_OnPointerUp?.Invoke(gameObject);
        }

        public override void OnPointerClick(PointerEventData _eventData)
        {
            m_OnPointerClick?.Invoke(gameObject);
        }

        public override void OnDrag(PointerEventData _eventData)
        {
            m_OnDrag?.Invoke(gameObject);
        }

        public override void OnDrop(PointerEventData _eventData)
        {
            m_OnDrop?.Invoke(gameObject);
        }

        public override void OnScroll(PointerEventData _eventData)
        {
            m_OnScroll?.Invoke(gameObject);
        }

        public override void OnUpdateSelected(BaseEventData _eventData)
        {
            m_OnUpdateSelected?.Invoke(gameObject);
        }

        public override void OnSelect(BaseEventData _eventData)
        {
            m_OnSelect?.Invoke(gameObject);
        }

        public override void OnDeselect(BaseEventData _eventData)
        {
            m_OnDeselect?.Invoke(gameObject);
        }

        public override void OnMove(AxisEventData _eventData)
        {
            m_OnMove?.Invoke(gameObject);
        }

        public override void OnInitializePotentialDrag(PointerEventData _eventData)
        {
            m_OnInitializePotentialDrag?.Invoke(gameObject);
        }

        public override void OnBeginDrag(PointerEventData _eventData)
        {
            m_OnBeginDrag?.Invoke(gameObject);
        }

        public override void OnEndDrag(PointerEventData _eventData)
        {
            m_OnEndDrag?.Invoke(gameObject);
        }

        public override void OnSubmit(BaseEventData _eventData)
        {
            m_OnSubmit?.Invoke(gameObject);
        }

        public override void OnCancel(BaseEventData _eventData)
        {
            m_OnCancel?.Invoke(gameObject);
        }
    }
}