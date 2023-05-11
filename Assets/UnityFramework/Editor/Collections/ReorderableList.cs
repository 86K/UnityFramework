namespace UnityFramework.Editor
{
    using UnityEditorInternal;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Array and list can be drag add and delete on inspector panel.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true, isFallback = true)]
    internal class ReorderableListExtension : Editor
    {
        static Dictionary<string, ReorderableListProperty> m_ReorderableLists;

        void OnEnable()
        {
            if (m_ReorderableLists == null)
            {
                m_ReorderableLists = new Dictionary<string, ReorderableListProperty>(64);
            }

            m_ReorderableLists.Clear();
        }

        void OnDisable()
        {
            if (m_ReorderableLists != null)
            {
                m_ReorderableLists.Clear();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var savedColor = GUI.color;
            var savedEnabled = GUI.enabled;
            var property = serializedObject.GetIterator();
            var isValid = property.NextVisible(true);
            if (isValid)
            {
                do
                {
                    GUI.color = savedColor;
                    GUI.enabled = savedEnabled;
                    DrawProperty(property);
                } while (property.NextVisible(false));
            }

            serializedObject.ApplyModifiedProperties();
        }

        void DrawProperty(SerializedProperty _property)
        {
            if (_property.name.Equals("m_Script") &&
                _property.type.Equals("PPtr<MonoScript>") &&
                _property.propertyType == SerializedPropertyType.ObjectReference &&
                _property.propertyPath.Equals("m_Script"))
            {
                GUI.enabled = false;
            }

            if (_property.isArray && _property.propertyType != SerializedPropertyType.String)
            {
                DrawArray(_property);
            }
            else
            {
                EditorGUILayout.PropertyField(_property, true);
            }
        }

        void DrawArray(SerializedProperty _property)
        {
            if (EditorGUILayout.Foldout(_property.isExpanded, _property.displayName, true) != _property.isExpanded)
            {
                _property.isExpanded = !_property.isExpanded;
            }

            if (_property.isExpanded)
            {
                GetReorderableList(_property).List.DoLayoutList();
            }
        }

        internal static void DrawReorderableList(ReorderableListProperty _listProperty)
        {
            var prop = _listProperty.Property;
            if (EditorGUILayout.Foldout(prop.isExpanded, prop.displayName, true) != prop.isExpanded)
            {
                prop.isExpanded = !prop.isExpanded;
            }

            if (prop.isExpanded)
            {
                _listProperty.List.DoLayoutList();
            }
        }

        ReorderableListProperty GetReorderableList(SerializedProperty _property)
        {
            ReorderableListProperty retVal;
            if (m_ReorderableLists.TryGetValue(_property.name, out retVal))
            {
                retVal.Property = _property;
                return retVal;
            }

            retVal = new ReorderableListProperty(_property);
            m_ReorderableLists[_property.name] = retVal;
            return retVal;
        }

        internal class ReorderableListProperty
        {
            public ReorderableList List { get; }

            public SerializedProperty Property
            {
                get { return List.serializedProperty; }
                set { List.serializedProperty = value; }
            }

            public ReorderableListProperty(SerializedProperty _property)
            {
                List = new ReorderableList(_property.serializedObject, _property, true, false, true, true)
                {
                    headerHeight = 0f
                };
                List.onCanRemoveCallback += OnCanRemove;
                List.drawElementCallback += OnDrawElement;
                List.elementHeightCallback += OnElementHeight;
            }

            bool OnCanRemove(ReorderableList _list)
            {
                return List.count > 0;
            }

            float OnElementHeight(int _id)
            {
                return 4f + Mathf.Max(EditorGUIUtility.singleLineHeight,
                    EditorGUI.GetPropertyHeight(Property.GetArrayElementAtIndex(_id), GUIContent.none, true));
            }

            void OnDrawElement(Rect _rect, int _index, bool _active, bool _focused)
            {
                if (Property.GetArrayElementAtIndex(_index).propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUI.LabelField(_rect, Property.GetArrayElementAtIndex(_index).displayName);
                }

                _rect.height = EditorGUI.GetPropertyHeight(Property.GetArrayElementAtIndex(_index), GUIContent.none, true);
                EditorGUI.PropertyField(_rect, Property.GetArrayElementAtIndex(_index), GUIContent.none, true);
            }
        }
    }
}