namespace UnityFramework.Runtime
{
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private sealed class Event : IReference
        {
            private object m_Sender;
            private T m_EventArgs;

            public object Sender => m_Sender;
            public T EventArgs => m_EventArgs;
            
            public Event()
            {
                m_Sender = null;
                m_EventArgs = null;
            }

            public static Event Create(object sender, T args)
            {
                // NOTICE 事件池的事件节点是请求的引用池创建的
                Event eventNode = ReferencePool.Acquire<Event>();
                eventNode.m_Sender = sender;
                eventNode.m_EventArgs = args;
                return eventNode;
            }
            
            public void Clear()
            {
                m_Sender = null;
                m_EventArgs = null;
            }
        }   
    }
}