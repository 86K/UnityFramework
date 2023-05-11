namespace UnityFramework.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.ProjectWindowCallback;
    using UnityEngine;

    public class CreateFileAsset : EndNameEditAction
    {
        public override void Action(System.Int32 _instanceId, System.String _pathName, System.String _resourceFile)
        {
            Object obj = CreateAssetFromTemplate(_pathName, _resourceFile);
            ProjectWindowUtil.ShowCreatedAsset(obj);
        }

        //Create asset from file template.
        Object CreateAssetFromTemplate(string _path, string _resourceFile)
        {
            StreamReader streamReader = new StreamReader(_resourceFile);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_path);
            //Replace .lua class name.
            text = text.Replace("#LuaClass", fileNameWithoutExtension);
            StreamWriter streamWriter = new StreamWriter(_path);
            streamWriter.Write(text);
            streamWriter.Close();
            AssetDatabase.ImportAsset(_path);
            return AssetDatabase.LoadAssetAtPath(_path, typeof(Object));
        }
    }
    
    public static class FileCreator
    {
        static void CreateAsset(string _fileName, string _templateName)
        {
            // [NOTE]
            // 1.The script name and root folder name cannt be modifyed.
            // 2.The root folder can be placed anywhere, this has no effect on this feature. 
            var path = GetScriptFolderPath("FileCreator", "/FileAsset") + "Templates/" + _templateName;
            if (!File.Exists(path))
            {
                Debug.Log($"File template {path} is not exist.");
                return;
            }

            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);
            string folder = AssetDatabase.GetAssetPath(objects[0]);
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0, 
                ScriptableObject.CreateInstance<CreateFileAsset>(),
                folder + "/" + _fileName, null, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        static string GetScriptFolderPath(string _scriptName, string _folderName)
        {
            string[] assets = AssetDatabase.FindAssets(_scriptName);
            for (int i = 0; i < assets.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[i]);
                if (path.Contains(_folderName) && path.Contains(_scriptName))
                {
                    return path.Substring(0, path.IndexOf(_scriptName, System.StringComparison.Ordinal));
                }
            }

            return null;
        }

        [MenuItem("itsxwz/Gen/Asset/Xml", false, 0)]
        public static void CreateXml()
        {
            CreateAsset("Xml.xml", "XmlTemplate.xml");
        }

        [MenuItem("itsxwz/Gen/Asset/Json", false, 1)]
        public static void CreateJson()
        {
            CreateAsset("Json.json", "JsonTemplate.json");
        }

        [MenuItem("itsxwz/Gen/Asset/Lua", false, 1)]
        public static void CreateLua()
        {
            CreateAsset("Lua.lua", "LuaTemplate.lua");
        }

        [MenuItem("itsxwz/Gen/Asset/Txt", false, 1)]
        public static void CreateTxt()
        {
            CreateAsset("Txt.txt", "TxtTemplate.txt");
        }

        //Do not directly create excel file through this way.
        //If is a .xlsx file, we can not open it.
        //If is a .xls file, the data is broken.
        //If is a .csv file, the data is error code.
    }
}