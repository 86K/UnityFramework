using System;
using UnityEngine;

namespace UnityFramework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("itsxwz/UnityFramework/Event")]
    public sealed class EventComponent : UnityFrameworkComponent
    {
        private IEventManager m_EventManager;

        public int EventHandlerCount => m_EventManager.EventHandlerCount;
        public int EventCount => m_EventManager.EventCount;

        protected override void Awake()
        {
            base.Awake();

            m_EventManager = UnityFrameworkEntrance.GetModule<IEventManager>();
            if (m_EventManager == null)
            {
                Log.Error("Event manager is invalid.");
            }
        }

        public void SetDefaultHandler(EventHandler<GameEventArgs> handler)
        {
            m_EventManager.SetDefaultHandler(handler);
        }

        public int GetEventHandlerCount(int id)
        {
            return m_EventManager.GetEventHandlerCount(id);
        }

        public bool CheckEventHandlerExist(int id, EventHandler<GameEventArgs> handler)
        {
            return m_EventManager.CheckEventHandlerExist(id, handler);
        }

        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventManager.Subscribe(id, handler);
        }

        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventManager.Unsubscribe(id, handler);
        }

        public void Fire(object sender, GameEventArgs args)
        {
            m_EventManager.Fire(sender, args);
        }

        public void FireNow(object sender, GameEventArgs args)
        {
            m_EventManager.FireNow(sender, args);
        }
    }
}