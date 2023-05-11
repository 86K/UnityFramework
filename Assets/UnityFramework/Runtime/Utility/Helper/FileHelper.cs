using System.IO;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public static class FileHelper
    {
        public static bool IsFileExisted(string _path)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return File.Exists(_path);
            }

            return false;
        }
    }
}