namespace UnityFramework.Editor
{
    using UnityEditor;
    
    [InitializeOnLoad]
    internal static class DisplayGUID
    {
        static DisplayGUID()
        {
            Editor.finishedDefaultHeaderGUI += DisplayGUIDIfPersistent;
        }

        static void DisplayGUIDIfPersistent(Editor _editor)
        {
            if (!EditorUtility.IsPersistent(_editor.target))
                return;

            var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_editor.target));
            var totalRect = EditorGUILayout.GetControlRect();
            var controlRect = EditorGUI.PrefixLabel(totalRect, EditorGUIUtility.TrTempContent("GUID"));

            if (_editor.targets.Length > 1)
                EditorGUI.LabelField(controlRect, EditorGUIUtility.TrTempContent("[Multiple objects selected]"));
            else
                EditorGUI.SelectableLabel(controlRect, guid);
        }
    }
}