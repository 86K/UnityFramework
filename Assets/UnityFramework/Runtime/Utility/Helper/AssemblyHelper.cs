using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityFramework.Runtime
{
    public static class AssemblyHelper
    {
        static readonly Assembly[] m_Assemblies = null;
        static readonly Dictionary<string, Type> m_CachedTypes = new Dictionary<string, Type>(StringComparer.Ordinal);

        static AssemblyHelper()
        {
            m_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// Get assemblies that has loaded.
        /// </summary>
        /// <returns></returns>
        public static Assembly[] GetAssemblies()
        {
            return m_Assemblies;
        }

        /// <summary>
        /// Get all types in loaded assemblies. 
        /// </summary>
        /// <returns></returns>
        public static Type[] GetTypes()
        {
            List<Type> results = new List<Type>();
            foreach (Assembly assembly in m_Assemblies)
            {
                results.AddRange(assembly.GetTypes());
            }

            return results.ToArray();
        }

        /// <summary>
        /// Get all types in loaded assemblies. 
        /// </summary>
        /// <param name="_types"></param>
        public static void GetTypes(List<Type> _types)
        {
            if (_types == null)
            {
                Debug.LogError("Results is invalid.");
                return;
            }

            _types.Clear();
            foreach (Assembly assembly in m_Assemblies)
            {
                _types.AddRange(assembly.GetTypes());
            }
        }

        /// <summary>
        /// Gets the specified type in the loaded assembly.
        /// </summary>
        /// <param name="_typeName"></param>
        /// <returns></returns>
        public static Type GetType(string _typeName)
        {
            if (string.IsNullOrEmpty(_typeName))
            {
                Debug.LogError("Type name is invalid.");
                return null;
            }

            Type type = null;
            if (m_CachedTypes.TryGetValue(_typeName, out type))
            {
                return type;
            }

            type = Type.GetType(_typeName);
            if (type != null)
            {
                m_CachedTypes.Add(_typeName, type);
                return type;
            }

            foreach (Assembly assembly in m_Assemblies)
            {
                type = Type.GetType(string.Format("{0}, {1}", _typeName, assembly.FullName));
                if (type != null)
                {
                    m_CachedTypes.Add(_typeName, type);
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the value of the attribute identifier inherited from Attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetAttributeFlagValue<T>() where T : Attribute
        {
            foreach (Type type in GetTypes())
            {
                if (!type.IsAbstract || !type.IsSealed)
                {
                    continue;
                }

                foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public |
                                                               BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (fieldInfo.FieldType == typeof(string) && fieldInfo.IsDefined(typeof(T), false))
                    {
                        return (string) fieldInfo.GetValue(null);
                    }
                }

                foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Static | BindingFlags.Public |
                                                                         BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (propertyInfo.PropertyType == typeof(string) && propertyInfo.IsDefined(typeof(T), false))
                    {
                        return (string) propertyInfo.GetValue(null, null);
                    }
                }
            }

            return null;
        }
    }
}