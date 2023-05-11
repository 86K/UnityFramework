namespace UnityFramework.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    
    public static class ClassAssetCreator
    {
        /// <summary>
        /// Create scriptable object as unity's asset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CreateAsset<T>() where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string[] names = typeof(T).ToString().Split('.');
            string assetName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + names[names.Length - 1] + ".asset");

            AssetDatabase.CreateAsset(asset, assetName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}