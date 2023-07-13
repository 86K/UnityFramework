using System;

namespace UnityFramework.Runtime
{
    /*
     * 底层框架模块属于UnityFrameworkModule
     */
    
    /// <summary>
    /// 事件管理器主要是对以GameEventArgs基类及其继承类相关的事件池进行操作
    /// </summary>
    internal sealed class EventManager : UnityFrameworkModule, IEventManager
    {
        private readonly EventPool<GameEventArgs> m_EventPool;
        
        public int EventHandlerCount => m_EventPool.EventHandlerCount;
        public int EventCount => m_EventPool.EventCount;

        internal override int Priority => UnityFrameworkModulePriority.Event;

        public EventManager()
        {
            // NOTICE 事件管理器实例化的时间池是允许无事件处理句柄和允许多个事件处理句柄
            m_EventPool =
                new EventPool<GameEventArgs>(EventPoolMode.AllowNoEventHandler | EventPoolMode.AllowMultiEventHandler);
        }
        
        internal override void Update(float logicTime, float realTime)
        {
            m_EventPool.Update(logicTime, realTime);
        }

        internal override void Shutdown()
        {
            m_EventPool.Shutdown();
        }
        
        public int GetEventHandlerCount(int id)
        {
            return m_EventPool.GetEventHandlerCount(id);
        }

        public bool CheckEventHandlerExist(int id, EventHandler<GameEventArgs> handler)
        {
            return m_EventPool.CheckEventHandlerExist(id, handler);
        }

        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
             m_EventPool.Subscribe(id, handler);
        }

        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventPool.Unsubscribe(id, handler);
        }

        public void SetDefaultHandler(EventHandler<GameEventArgs> handler)
        {
            m_EventPool.SetDefaultHandler(handler);
        }

        public void Fire(object sender, GameEventArgs args)
        {
            m_EventPool.Fire(sender, args);
        }

        public void FireNow(object sender, GameEventArgs args)
        {
            m_EventPool.FireNow(sender, args);
        }
    }
}