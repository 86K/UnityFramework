namespace UnityFramework.Editor
{
    using System;
    using System.Diagnostics;
    using UnityEditor;
    using UnityEngine;
    
    /// <summary>
    /// Open unity's specil folders by explorer.exe.
    /// </summary>
    internal static class OpenFolder
    {
        [MenuItem("itsxwz/Open folder/Data path", false, 1)]
        internal static void OpenDataPathFolder()
        {
            Execute(Application.dataPath);
        }

        [MenuItem("itsxwz/Open folder/StreamingAssets path", false, 1)]
        internal static void OpenStreamingAssetsPathFolder()
        {
            Execute(Application.streamingAssetsPath);
        }

        [MenuItem("itsxwz/Open folder/Temporary path", false, 1)]
        internal static void OpenTemporaryPathFolder()
        {
            Execute(Application.temporaryCachePath);
        }

        [MenuItem("itsxwz/Open folder/Persistent Data path", false, 1)]
        internal static void OpenPersistentDataPathFolder()
        {
            Execute(Application.persistentDataPath);
        }

        static void Execute(string _folder)
        {
            _folder = string.Format("\"{0}\"", _folder);
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", _folder.Replace('/', '\\'));
                    break;

                case RuntimePlatform.OSXEditor:
                    Process.Start("open", _folder);
                    break;

                default:
                    throw new Exception($"Platform '{Application.platform}' is not support open folder '{_folder}'.");
            }
        }
    }
}