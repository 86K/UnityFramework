namespace UnityFramework.Editor
{
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;
    
    [CustomEditor(typeof(BaseAsset), true)]
    public class BaseDictAssetGUI : Editor
    {
        BaseAsset m_BaseDictAsset;
        SerializedProperty m_Keys;
        SerializedProperty m_Values;
        ReorderableList m_ReorderableList;
        Vector2 m_ScrollPos;
        int m_LastSelection;

        public void OnEnable()
        {
            m_BaseDictAsset = target as BaseAsset;
            m_Keys = serializedObject.FindProperty("keys");
            m_Values = serializedObject.FindProperty("values");
            m_ReorderableList = new ReorderableList(serializedObject, m_Keys, true, true, true, true)
            {
                drawHeaderCallback = DrawGUIHeader,
                drawElementCallback = DrawGUIElement,
                onAddCallback = OnAddElement,
                onRemoveCallback = OnRemoveElement,
                onReorderCallback = OnReorderElement,
                onSelectCallback = OnSelectElement,
            };
        }

        public override void OnInspectorGUI()
        {
            GUIUtil.ScriptableObjectTitle(target as ScriptableObject);

            serializedObject.Update();
            m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);
            m_ReorderableList.DoLayoutList();
            GUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGUIHeader(Rect _rect)
        {
            _rect = GUIUtil.DrawReorderableListHeader(_rect, m_ReorderableList);
            Rect keyRect, valueRect;
            SplitRect(_rect, out keyRect, out valueRect);

            DrawKeyLabel(keyRect);
            DrawValueLabel(valueRect);
        }

        private void DrawGUIElement(Rect _rect, int _index, bool _isActive, bool _isFocused)
        {
            SerializedProperty key = m_Keys.GetArrayElementAtIndex(_index);
            while (m_Values.arraySize < m_Keys.arraySize)
            {
                m_Values.InsertArrayElementAtIndex(m_Values.arraySize);
            }

            SerializedProperty value = m_Values.GetArrayElementAtIndex(_index);

            _rect = GUIUtil.DrawReorderableListElement(_rect, _index, serializedObject, m_Keys, m_Values);
            Rect keyRect, valueRect;
            SplitRect(_rect, out keyRect, out valueRect);

            Color original = GUI.backgroundColor;
            if (m_BaseDictAsset.IsDuplicateKey(_index))
            {
                GUI.backgroundColor = Color.red;
            }

            DrawKeyProperty(keyRect, key);
            GUI.backgroundColor = original;
            DrawValueProperty(valueRect, value);
        }

        private void DrawKeyLabel(Rect _rect, string _keyLabel = "key")
        {
            EditorGUI.LabelField(_rect, _keyLabel);
        }

        private void DrawValueLabel(Rect _rect, string _valueLabel = "Value")
        {
            EditorGUI.LabelField(_rect, _valueLabel);
        }

        private void DrawKeyProperty(Rect _rect, SerializedProperty _keyProperty)
        {
            EditorGUI.PropertyField(_rect, _keyProperty, GUIContent.none);
        }

        private void DrawValueProperty(Rect _rect, SerializedProperty _valueProperty)
        {
            EditorGUI.PropertyField(_rect, _valueProperty, GUIContent.none);
        }

        private void OnAddElement(ReorderableList _list)
        {
            int index = m_Keys.arraySize;
            m_Keys.InsertArrayElementAtIndex(index);
            while (m_Values.arraySize < m_Keys.arraySize)
            {
                m_Values.InsertArrayElementAtIndex(m_Values.arraySize);
            }

            _list.index = index;
        }

        private void OnRemoveElement(ReorderableList _list)
        {
            int index = _list.index;
            if (index < 0)
            {
                return;
            }

            m_Keys.DeleteArrayElementAtIndex(index);
            m_Values.DeleteArrayElementAtIndex(index);
            if (_list.index >= _list.count)
            {
                _list.index--;
            }
        }

        private void OnReorderElement(ReorderableList _list)
        {
            m_Values.MoveArrayElement(m_LastSelection, _list.index);
        }

        private void OnSelectElement(ReorderableList _list)
        {
            m_LastSelection = _list.index;
        }

        private void SplitRect(Rect _rect, out Rect _left, out Rect _right)
        {
            float margin = 5;
            _rect.width -= margin;

            _left = _right = new Rect(_rect);
            _left.width *= m_BaseDictAsset.keyRectWidth;
            _right.width *= 1 - m_BaseDictAsset.keyRectWidth;

            _left.height = EditorGUIUtility.singleLineHeight;
            _right.x += _left.width + margin;
            _right.height = EditorGUIUtility.singleLineHeight;
        }
    }
}