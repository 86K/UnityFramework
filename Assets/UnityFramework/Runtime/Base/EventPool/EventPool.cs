using System;
using System.Collections.Generic;

namespace UnityFramework.Runtime
{
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private readonly UnityFrameworkMultiDictionary<int, EventHandler<T>> m_EventHandlers;
        private readonly Queue<Event> m_Events;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> m_CachedNodes;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> m_TempNodes;
        private readonly EventPoolMode m_EventPoolMode;
        private EventHandler<T> m_DefaultHandler;

        public int EventHandlerCount => m_EventHandlers.Count;
        public int EventCount => m_Events.Count;

        public EventPool(EventPoolMode mode)
        {
            m_EventHandlers = new UnityFrameworkMultiDictionary<int, EventHandler<T>>();
            m_Events = new Queue<Event>();
            m_CachedNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            m_TempNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            m_EventPoolMode = mode;
            m_DefaultHandler = null;
        }

        public void Update(float logicSecond, float realSecond)
        {
            lock (m_Events)
            {
                while (m_Events.Count > 0)
                {
                    Event eventNode = m_Events.Dequeue();
                    HandleEvent(eventNode.Sender, eventNode.EventArgs);
                    // 处理完事件后，回收“Event”到引用池
                    ReferencePool.Release(eventNode);
                }
            }
        }
        
        public void SetDefaultHandler(EventHandler<T> handler)
        {
            m_DefaultHandler = handler;
        }

        public int GetEventHandlerCount(int id)
        {
            if (m_EventHandlers.TryGetValue(id, out var range))
            {
                return range.Count;
            }

            return 0;
        }

        public bool CheckEventHandlerExist(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            return m_EventHandlers.Contains(id, handler);
        }

        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
                throw new Exception("Event handler is invalid.");

            if (!m_EventHandlers.Contains(id))
            {
                m_EventHandlers.Add(id, handler);
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowMultiEventHandler) != EventPoolMode.AllowMultiEventHandler)
            {
                throw new Exception($"Event '{id}' not allow multi handler.");
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowDuplicateEventHandler) != EventPoolMode
                         .AllowDuplicateEventHandler && CheckEventHandlerExist(id, handler))
            {
                throw new Exception($"Event '{id}' not allow duplicate handler.");
            }
            else
            {
                m_EventHandlers.Add(id, handler);
            }
        }
        
        public void Unsubscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (m_CachedNodes.Count > 0)
            {
                foreach (KeyValuePair<object, LinkedListNode<EventHandler<T>>> cachedNode in m_CachedNodes)
                {
                    if (cachedNode.Value != null && cachedNode.Value.Value == handler)
                    {
                        m_TempNodes.Add(cachedNode.Key, cachedNode.Value.Next);
                    }
                }

                if (m_TempNodes.Count > 0)
                {
                    foreach (KeyValuePair<object, LinkedListNode<EventHandler<T>>> cachedNode in m_TempNodes)
                    {
                        m_CachedNodes[cachedNode.Key] = cachedNode.Value;
                    }

                    m_TempNodes.Clear();
                }
            }

            if (!m_EventHandlers.Remove(id, handler))
            {
                throw new Exception($"Event '{id}' not exists specified handler.");
            }
        }
        
        /// <summary>
        /// 抛出事件：线程安全，即使不在主线程中抛出，也会在主线程中回调事件处理函数，事件会在抛出的下一帧分发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        public void Fire(object sender, T e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            Event eventNode = Event.Create(sender, e);
            lock (m_Events)
            {
                m_Events.Enqueue(eventNode);
            }
        }
        
        /// <summary>
        /// 立即抛出事件：非线程安全，事件会立刻分发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        public void FireNow(object sender, T e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            HandleEvent(sender, e);
        }

        public void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
        }

        public void Shutdown()
        {
            Clear();
            m_EventHandlers.Clear();
            m_CachedNodes.Clear();
            m_TempNodes.Clear();
            m_DefaultHandler = null;
        }

        private void HandleEvent(object sender, T args)
        {
            bool noHandlerException = false;
            UnityFrameworkLinkedListRange<EventHandler<T>> range = default
                (UnityFrameworkLinkedListRange<EventHandler<T>>);
            if (m_EventHandlers.TryGetValue(args.Id, out range))
            {
                LinkedListNode<EventHandler<T>> current = range.First;
                while (current != null && current != range.Last)
                {
                    m_CachedNodes[args] = current.Next != range.Last ? current.Next : null;
                    current.Value(sender, args);
                    current = m_CachedNodes[args];
                }

                m_CachedNodes.Remove(args);
            }
            else if (m_DefaultHandler != null)
            {
                m_DefaultHandler(sender, args);
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowNoEventHandler) == 0)
            {
                noHandlerException = true;
            }

            // NOTICE 处理完事件后，回收"T"到引用池
            ReferencePool.Release(args);

            if (noHandlerException)
            {
                throw new Exception($"Event '{args.Id}' not allow no handler.");
            }
        }
    }
}