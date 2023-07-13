using UnityEditor;

namespace UnityFramework.Editor
{
    public abstract class UnityFrameworkInspector : UnityEditor.Editor
    {
        private bool IsCompiling = false;

        public override void OnInspectorGUI()
        {
            if (IsCompiling && !EditorApplication.isCompiling)
            {
                IsCompiling = false;
                OnCompileComplete();
            }
            else if (!IsCompiling && EditorApplication.isCompiling)
            {
                IsCompiling = true;
                OnCompileStart();
            }
        }

        protected virtual void OnCompileStart()
        {
        }

        protected virtual void OnCompileComplete()
        {
        }

        protected bool IsPrefabInHierarchy(UnityEngine.Object obj)
        {
            if (obj == null)
                return false;

#if UNITY_2018_3_OR_NEWER
            return PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.Regular;
#else
            return PrefabUtility.GetPrefabType(obj) != PrefabType.Prefab;
#endif
        }
    }
}