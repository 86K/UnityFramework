using System;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// NOTICE
    /// 事件管理器处理的T，是GameEventArgs类型，其为游戏中所有事件类型的基类
    /// </summary>
    public interface IEventManager
    {
        int EventHandlerCount { get; }

        int EventCount { get; }

        int GetEventHandlerCount(int id);

        bool CheckEventHandlerExist(int id, EventHandler<GameEventArgs> handler);

        void Subscribe(int id, EventHandler<GameEventArgs> handler);

        void Unsubscribe(int id, EventHandler<GameEventArgs> handler);

        void SetDefaultHandler(EventHandler<GameEventArgs> handler);

        void Fire(object sender, GameEventArgs args);

        void FireNow(object sender, GameEventArgs args);
    }
}