using UnityEditor;

namespace UnityFramework.Editor
{
    /// <summary>
    /// [NOTE]
    /// This is an example show how to create a scriptable object class as unity's asset.
    /// </summary>
    public static class TestCreateClassAsset
    {
        [MenuItem("Assets/Create/UnityFramework/Example_ScriptableObjectClass", false, 50)]
        public static void CreateAppConfig()
        {
            ClassAssetCreator.CreateAsset<TestScriptableObject>();
        }
    }
}