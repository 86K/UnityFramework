using System;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 所有的UnityFrameworkComponent从入口注册和获取
    /// </summary>
    public static class GameEntry
    {
        private static readonly UnityFrameworkLinkedList<UnityFrameworkComponent> m_UnityFrameworkComponents = new UnityFrameworkLinkedList<UnityFrameworkComponent>();

        public static T GetComponent<T>() where T : UnityFrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public static UnityFrameworkComponent GetComponent(Type type)
        {
            var current = m_UnityFrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                    return current.Value;

                current = current.Next;
            }
            return null;
        }

        public static UnityFrameworkComponent GetComponent(string typeName)
        {
            var current = m_UnityFrameworkComponents.First;
            while (current != null)
            {
                var type = current.Value.GetType();
                if (type.FullName == typeName || type.Name == typeName)
                    return current.Value;

                current = current.Next;
            }
            return null;
        }

        public static void Register(UnityFrameworkComponent unityFrameworkComponent)
        {
            if(unityFrameworkComponent == null)
            {
                Log.Error("Game Framework component is invalid.");
                return;
            }

            var type = unityFrameworkComponent.GetType();
            var current = m_UnityFrameworkComponents.First;
            while(current != null)
            {
                if(current.Value.GetType() == type)
                {
                    Log.Error($"Unity Framework Component type '{type.FullName}' is already exist.");
                    return;
                }
                current = current.Next;
            }
            m_UnityFrameworkComponents.AddLast(unityFrameworkComponent);
        }
    }
}
