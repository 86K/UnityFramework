namespace UnityFramework.Editor
{
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;
    
    internal class DuplicateAssetsImportChecker
    {
        internal string OriginalAssetPath
        {
            get { return Path.Combine(m_DirectoryName, m_Filename + "." + m_Extension); }
        }

        internal bool m_Exists;
        internal string m_Filename;
        internal string m_Extension;

        string m_DirectoryName;
        string m_AssetPath;
        const string Pattern = "^(?<name>.*)\\s\\d+\\.(?<m_Extension>.*)$";

        internal DuplicateAssetsImportChecker(string _assetPath)
        {
            m_AssetPath = _assetPath;
            m_DirectoryName = Path.GetDirectoryName(_assetPath);
            var match = Regex.Match(Path.GetFileName(_assetPath), Pattern);

            m_Exists = match.Success;

            if (m_Exists)
            {
                m_Filename = match.Groups["name"].Value;
                m_Extension = match.Groups["m_Extension"].Value;
            }
        }

        internal void Overwrite()
        {
            FileUtil.ReplaceFile(m_AssetPath, OriginalAssetPath);
            Delete();
            AssetDatabase.ImportAsset(OriginalAssetPath);
        }

        internal void Delete()
        {
            AssetDatabase.DeleteAsset(m_AssetPath);
        }
    }

    internal class DuplicateImport : AssetPostprocessor
    {
        const string Message = "\"{0}.{1}\"is already exist, would you want to update?";

        static void OnPostprocessAllAssets(string[] _importedAssets, string[] _deletedAssets, string[] _movedAssets, string[] _movedFromPath)
        {
            if (Event.current == null || Event.current.type != EventType.DragPerform)
                return;

            foreach (var assetPath in _importedAssets)
            {
                var checker = new DuplicateAssetsImportChecker(assetPath);
                if (checker.m_Exists)
                {
                    var overwriteMessage =
                        string.Format(Message, checker.m_Filename, checker.m_Extension);
                    
                    var result = EditorUtility.DisplayDialogComplex(checker.OriginalAssetPath, overwriteMessage,
                        "Replace",
                        "Keep",
                        "Cancel");

                    if (result == 0)
                    {
                        checker.Overwrite();
                    }
                    else if (result == 2)
                    {
                        checker.Delete();
                    }
                }
            }
        }
    }
}