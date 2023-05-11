namespace UnityFramework.Editor
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Displays file and folder information recursively on the inspection panel.
    /// Assets panel use one column layout.
    /// </summary>
    [CustomEditor(typeof(DefaultAsset))]
    internal class DisplayFolderCatalog : Editor
    {
        class Data
        {
            public bool IsSelected = false;
            public int Indent = 0;
            public GUIContent Content;
            public string AssetPath;
            public readonly List<Data> Childs = new List<Data>();
        }

        Data m_Data;
        Data m_SelectedData;

        void OnEnable()
        {
            if (Directory.Exists(AssetDatabase.GetAssetPath(target)))
            {
                m_Data = new Data();
                LoadFiles(m_Data, AssetDatabase.GetAssetPath(Selection.activeObject));
            }
        }

        void DrawData(Data _data)
        {
            if (_data.Content != null)
            {
                EditorGUI.indentLevel = _data.Indent;
                DrawGUIData(_data);
            }

            foreach (var child in _data.Childs)
            {
                if (child.Content != null)
                {
                    EditorGUI.indentLevel = child.Indent;
                    if (child.Childs.Count > 0)
                        DrawData(child);
                    else
                        DrawGUIData(child);
                }
            }
        }

        void DrawGUIData(Data _data)
        {
            GUIStyle style = "Label";
            Rect rt = GUILayoutUtility.GetRect(_data.Content, style);
            if (_data.IsSelected)
            {
                EditorGUI.DrawRect(rt, Color.gray);
            }

            rt.x += (16 * EditorGUI.indentLevel);
            if (GUI.Button(rt, _data.Content, style))
            {
                if (m_SelectedData != null)
                {
                    m_SelectedData.IsSelected = false;
                }

                _data.IsSelected = true;
                m_SelectedData = _data;

                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(_data.AssetPath));
            }
        }

        GUIContent GetGUIContent(string _path)
        {
            Object asset = AssetDatabase.LoadAssetAtPath(_path, typeof(Object));
            if (asset)
            {
                return new GUIContent(asset.name, AssetDatabase.GetCachedIcon(_path));
            }

            return null;
        }

        void LoadFiles(Data _data, string _currentPath, int _indent = 0)
        {
            GUIContent content = GetGUIContent(_currentPath);

            if (content != null)
            {
                _data.Indent = _indent;
                _data.Content = content;
                _data.AssetPath = _currentPath;
            }

            foreach (var path in Directory.GetFiles(_currentPath))
            {
                content = GetGUIContent(path);
                if (content != null)
                {
                    Data child = new Data { Indent = _indent + 1, Content = content, AssetPath = path };
                    _data.Childs.Add(child);
                }
            }

            foreach (var path in Directory.GetDirectories(_currentPath))
            {
                Data childDir = new Data();
                _data.Childs.Add(childDir);
                LoadFiles(childDir, path, _indent + 1);
            }
        }

        public override void OnInspectorGUI()
        {
            if (Directory.Exists(AssetDatabase.GetAssetPath(target)))
            {
                GUI.enabled = true;
                EditorGUIUtility.SetIconSize(Vector2.one * 16);
                DrawData(m_Data);
            }
        }
    }
}