namespace UnityFramework.Editor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    internal static class DisplayHierarchyIcons
    {
        const int MAX_ICON_NUM = 4;

        static readonly List<System.Type> m_HideTypes =
            new List<System.Type>() { typeof(Transform), typeof(ParticleSystemRenderer), typeof(CanvasRenderer), };

        static Transform m_OffsetObject = null;
        static int m_Offset = 0;

        [InitializeOnLoadMethod]
        internal static void Init()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyGUI;
        }

        static void HierarchyGUI(int _instanceID, Rect _rect)
        {
            Object tempObj = EditorUtility.InstanceIDToObject(_instanceID);
            if (!tempObj)
            {
                return;
            }

            _rect.width += _rect.x;
            _rect.x = 0;

            GameObject obj = tempObj as GameObject;
            if (obj == null)
                return;

            List<Component> coms = new List<Component>(obj.GetComponents<Component>());
            for (int i = 0; i < coms.Count; i++)
            {
                if (!coms[i])
                {
                    continue;
                }

                if (TypeCheck(coms[i].GetType()))
                {
                    coms.RemoveAt(i);
                    i--;
                }
            }

            int iconSize = 16;
            int y = 1;
            int offset = obj.transform == m_OffsetObject ? m_Offset : 0;

            for (int i = 0; i + offset < coms.Count && i < MAX_ICON_NUM; i++)
            {
                Component com = coms[i + offset];
                Texture2D texture = AssetPreview.GetMiniThumbnail(com);
                if (texture)
                {
                    GUI.DrawTexture(new Rect(_rect.width - (iconSize + 1) * (i + 1), _rect.y + y, iconSize, iconSize),
                        texture);
                }
            }

            if (coms.Count == MAX_ICON_NUM + 1)
            {
                Texture2D texture = AssetPreview.GetMiniThumbnail(coms[coms.Count - 1]);
                if (texture)
                {
                    GUI.DrawTexture(
                        new Rect(_rect.width - (iconSize + 1) * (coms.Count - 1 + 1), _rect.y + y, iconSize, iconSize),
                        texture);
                }
            }
            else if (coms.Count > MAX_ICON_NUM)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = 9, alignment = TextAnchor.MiddleCenter };
                if (GUI.Button(new Rect(_rect.width - (iconSize + 2) * (MAX_ICON_NUM + 1), _rect.y + y, 22, iconSize),
                        "•••",
                        style))
                {
                    if (m_OffsetObject != obj.transform)
                    {
                        m_OffsetObject = obj.transform;
                        m_Offset = 0;
                    }

                    m_Offset += MAX_ICON_NUM;
                    if (m_Offset >= coms.Count)
                    {
                        m_Offset = 0;
                    }
                }
            }
        }

        static bool TypeCheck(System.Type _type)
        {
            foreach (var t in m_HideTypes)
            {
                if (_type == t || _type.IsSubclassOf(t))
                {
                    return true;
                }
            }

            return false;
        }
    }
}