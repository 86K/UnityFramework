namespace UnityFramework.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public static class FolderHelper
    {
        /// <summary>
        /// Open folder by path.
        /// </summary>
        /// <param name="_path"></param>
        public static void OpenFolder(string _path)
        {
            if (!Directory.Exists(_path))
            {
                Debug.Log($"Folder path: '{_path}' is not exist.");
                return;
            }

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    System.Diagnostics.Process.Start("Explorer.exe", _path.Replace('/', '\\'));
                    break;

                case RuntimePlatform.OSXEditor:
                    System.Diagnostics.Process.Start("open", _path);
                    break;
            }
        }

        /// <summary>
        /// Copy the entire folder to another.
        /// </summary>
        /// <param name="_fromPath"></param>
        /// <param name="_toPath"></param>
        public static void CopyFolder(string _fromPath, string _toPath)
        {
            try
            {
                if (!Directory.Exists(_toPath))
                {
                    Directory.CreateDirectory(_toPath);
                }

                string[] files = Directory.GetFiles(_fromPath);
                foreach (string file in files)
                {
                    string pFilePath = _toPath + "\\" + Path.GetFileName(file);
                    if (File.Exists(pFilePath))
                        continue;
                    File.Copy(file, pFilePath, true);
                }

                string[] dirs = Directory.GetDirectories(_fromPath);
                foreach (string dir in dirs)
                {
                    CopyFolder(dir, _toPath + "\\" + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// Create folder by path.
        /// </summary>
        /// <param name="_folderPath"></param>
        public static void CreateFolder(string _folderPath)
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        /// <summary>
        /// Delete the entire folder.
        /// </summary>
        /// <param name="_path"></param>
        public static void DeleteFolder(string _path)
        {
            if (Directory.Exists(_path))
            {
                Directory.Delete(_path, true);
            }
        }

        /// <summary>
        /// Get all file path by file type(.exe .jpg) and folder path.
        /// </summary>
        /// <param name="_folderPath"></param>
        /// <param name="_fileType"></param>
        /// <param name="_filePath"></param>
        public static void GetFilesPath(string _folderPath, string _fileType, ref List<string> _filePath)
        {
            if (Directory.Exists(_folderPath))
            {
                foreach (string path in Directory.GetFiles(_folderPath))
                {
                    if (Path.GetExtension(path) == _fileType)
                    {
                        if (!_filePath.Contains(path))
                        {
                            _filePath.Add(path);
                        }
                    }
                }

                foreach (string directory in Directory.GetDirectories(_folderPath))
                {
                    foreach (string path in Directory.GetFiles(directory))
                    {
                        if (Path.GetExtension(path) == _fileType)
                        {
                            if (!_filePath.Contains(path))
                            {
                                _filePath.Add(path);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get file names by file paths.
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_fileName"></param>
        public static void GetFilesName(ref List<string> _filePath, ref List<string> _fileName)
        {
            for (int i = 0; i < _filePath.Count; i++)
            {
                string[] strs = _filePath[i].Split('/');
                string fileName = strs[strs.Length - 1].Split('.')[0];
                if (!_fileName.Contains(fileName))
                {
                    _fileName.Add(fileName);
                }
            }
        }
    }
}