using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{
    [CustomEditor(typeof(LocaliseLanguage))]
    public class LocaliseLanguageGUI : BaseDictAssetGUI
    {
        //[NOTE] Add a language type name here to expand the scriptable object contain types.
        readonly string[] m_LanguageNames = new string[] {"zh_CN", "en_US"};

        protected void SplitRect(Rect _rect, out Rect _left, out Rect _right)
        {
            _rect.height = EditorGUIUtility.singleLineHeight;
            float margin = 5;

            _left = _right = new Rect(_rect);
            float gridWidth = _rect.width / (m_LanguageNames.Length + 1);
            _left.width = gridWidth - margin;

            _right.x += gridWidth;
            _right.width = gridWidth * m_LanguageNames.Length;
        }

        protected void DrawValueLabel(Rect _rect, string _valueLabel = "Value")
        {
            _rect.height = EditorGUIUtility.singleLineHeight;
            _rect.width /= m_LanguageNames.Length;
            for (int i = 0; i < m_LanguageNames.Length; i++)
            {
                EditorGUI.LabelField(_rect, m_LanguageNames[i]);
                _rect.x += _rect.width;
            }
        }

        protected void DrawValueProperty(Rect _rect, SerializedProperty _valueProperty)
        {
            _rect.height = EditorGUIUtility.singleLineHeight;
            float margin = 5;

            float width = _rect.width / m_LanguageNames.Length;
            _rect.width = width - margin;

            for (int i = 0; i < m_LanguageNames.Length; i++)
            {
                EditorGUI.PropertyField(_rect, _valueProperty.FindPropertyRelative(m_LanguageNames[i]), GUIContent.none);
                _rect.x += width;
            }
        }
    }
}