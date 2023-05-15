using System;
using System.Runtime.InteropServices;

namespace UnityFramework.Runtime
{
    [StructLayout(LayoutKind.Auto)]
    internal struct TypeNamePair : IEquatable<TypeNamePair>
    {
        private readonly Type m_Type;
        private readonly string m_Name;

        public Type Type => m_Type;
        public string Name => m_Name;
        
        public TypeNamePair(Type type):this(type, string.Empty){}

        public TypeNamePair(Type type, string name)
        {
            m_Type = type ?? throw new Exception("Type is invalid.");
            m_Name = name ?? string.Empty;
        }
        
        public bool Equals(TypeNamePair other)
        {
            return m_Type == other.m_Type && m_Name == other.m_Name;
        }

        public static bool operator ==(TypeNamePair a, TypeNamePair b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TypeNamePair a, TypeNamePair b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return m_Type.GetHashCode() ^ m_Name.GetHashCode();
        }

        public override string ToString()
        {
            if (m_Type == null)
            {
                throw new Exception("Type is invalid.");
            }

            string typeName = m_Type.FullName;
            return string.IsNullOrEmpty(m_Name) ? typeName : string.Format($"{typeName}.{m_Name}");
        }
    }
}