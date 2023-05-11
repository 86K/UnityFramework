namespace UnityFramework.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    internal sealed class ProjectFoldersGenerator : EditorWindow
    {
        [Flags]
        enum Options
        {
            //Unity special folders
            Editor = 1,
            Plugins,
            StreamingAssets,

            //self defined folders
            Art = 101,
            Res,
            Program,
        }

        static readonly Dictionary<int, List<string>> DefinedPaths = new Dictionary<int, List<string>>
        {
            //Define Unity special folders
            { (int)Options.Editor, new List<string>() { "Editor" } },
            { (int)Options.Plugins, new List<string>() { "Plugins" } },
            { (int)Options.StreamingAssets, new List<string>() { "StreamingAssets" } },

            {
                //Define artister's raw file folders
                (int)Options.Art, new List<string>
                {
                    "Shaders",
                    "Fonts",
                    "Models",
                    "Textures",
                    "Materials",
                    "Animations",
                    "Sounds",
                }
            },
            {
                //Define resource file folders
                (int)Options.Res, new List<string>
                {
                    "Configs",
                    "Prefabs",
                    "Scenes",
                    "Sprites",
                    "Audios",
                }
            },
            {
                //Defined programmer folders
                (int)Options.Program, new List<string>
                {
                    "Editor",
                    "Runtime",
                }
            }
        };

        const Options DefaultOptions = (Options)(-1);
        static Options m_Options;

        [MenuItem("itsxwz/Gen/Project folders", false, 100)]
        static void Gen()
        {
            m_Options = DefaultOptions;
            Generate(m_Options);
        }

        static void GenerateItem(int _item)
        {
            var fullPath = Path.Combine(Application.dataPath, ((Options)_item).ToString());
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            if (DefinedPaths.ContainsKey(_item))
            {
                foreach (var subFolder in DefinedPaths[_item])
                {
                    var path = Path.Combine(fullPath, subFolder);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
            }
        }

        static void Generate(Options _options)
        {
            if (_options == 0)
            {
                return;
            }

            try
            {
                foreach (Options item in Enum.GetValues(typeof(Options)))
                {
                    if ((_options & item) != 0)
                    {
                        GenerateItem((int)item);
                    }
                }

                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                AssetDatabase.Refresh();
                Debug.LogError($"Generate project folder failed, cause :{ex}");
            }
        }
    }
}