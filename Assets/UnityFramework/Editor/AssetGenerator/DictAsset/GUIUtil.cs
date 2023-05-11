namespace UnityFramework.Editor
{
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;
    
    public class GUIUtil : MonoBehaviour
    {
        const float m_DragHandleWidth = 15;
        const float m_ReorderableListPrefixWidth = 30;

        public static Rect DrawReorderableListHeader(Rect _rect, ReorderableList _reorderableList)
        {
            float indentWidth = _reorderableList.draggable ? (m_DragHandleWidth + m_ReorderableListPrefixWidth) : m_ReorderableListPrefixWidth;
            _rect.x += indentWidth;
            _rect.width -= indentWidth;
            return _rect;
        }

        public static Rect DrawReorderableListElement(Rect _rect, int _index, SerializedObject _serializedObject,
            params SerializedProperty[] _serializedProperties)
        {
            GUI.Button(new Rect(_rect.x, _rect.y, m_ReorderableListPrefixWidth, EditorGUIUtility.singleLineHeight), _index.ToString("00"),
                EditorStyles.label);
            _rect.x += m_ReorderableListPrefixWidth;
            _rect.width -= m_ReorderableListPrefixWidth;
            return _rect;
        }

        public static void ScriptableObjectTitle(ScriptableObject _target, bool _showTarget = false)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(_target), typeof(MonoScript), false);
            if (_showTarget)
            {
                EditorGUILayout.ObjectField("Asset", _target, typeof(System.Object), false);
            }

            GUI.enabled = true;
        }
    }
}