using System;
using System.Collections.Generic;

namespace UnityFramework.Runtime
{
    public static class UnityFrameworkEntrance
    {
        private static readonly UnityFrameworkLinkedList<UnityFrameworkModule> m_Modules =
            new UnityFrameworkLinkedList<UnityFrameworkModule>();

        public static void Update(float logicTime, float realTime)
        {
            foreach (var module in m_Modules)
            {
                module.Update(logicTime, realTime);
            }
        }
        
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new Exception($"You must get module by interface, but '{interfaceType.FullName}' is not.");
            }

            // NOTE 接口类型的全名包含命名空间
            if (interfaceType.FullName != null && !interfaceType.FullName.StartsWith("UnityFramework.", StringComparison.Ordinal))
            {
                throw new Exception($"You must get a Unity Framework module, but '{interfaceType.FullName}' is not.");
            }

            string moduleName = $"{interfaceType.Namespace}.{interfaceType.Name.Substring(1)}";
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new Exception($"Can not find Unity Framework module type '{moduleName}'.");
            }

            return GetModule(moduleType) as T;
        }

        private static UnityFrameworkModule GetModule(Type moduleType)
        {
            foreach (UnityFrameworkModule module in m_Modules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }
        
        private static UnityFrameworkModule CreateModule(Type moduleType)
        {
            UnityFrameworkModule module = (UnityFrameworkModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new Exception($"Can not create module '{moduleType.FullName}'.");
            }

            LinkedListNode<UnityFrameworkModule> current = m_Modules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_Modules.AddBefore(current, module);
            }
            else
            {
                m_Modules.AddLast(module);
            }

            return module;
        }

        public static void Shutdown()
        {
            for (LinkedListNode<UnityFrameworkModule> current = m_Modules.Last;
                 current != null;
                 current = current.Previous)
            {
                current.Value.Shutdown();
            }

            m_Modules.Clear();
            // NOTE 框架相关的都要清理
            // ReferencePool.ClearAll();
            Util.Marshal.FreeCachedHGlobal();
        }
    }
}