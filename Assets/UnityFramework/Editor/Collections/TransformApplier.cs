namespace UnityFramework.Editor
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// You can change and apply multi game objects' transform's properties while playing on editor mode.
    /// </summary>
    internal class TransformApplier : Editor
    {
        [MenuItem("itsxwz/Transform/Save Transforms Data", false, 100)]
        public static void Record()
        {
            var allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            var previousSelection = Selection.objects;
            Selection.objects = allObjects;
            var transforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
            Selection.objects = previousSelection;
            foreach (var trans in transforms)
            {
                string key = trans.GetHashCode().ToString();
                if (trans != null)
                {
                    Vector3 pos = trans.localPosition;
                    Vector3 angle = trans.localEulerAngles;
                    Vector3 scale = trans.localScale;

                    string value = pos.x + "/" + pos.y + "/" + pos.z + "/"
                                   + angle.x + "/" + angle.y + "/" + angle.z + "/"
                                   + scale.x + "/" + scale.y + "/" + scale.z;

                    // Avoid effect other projects, we use PlayerPrefs rather then EditorPrefs.
                    PlayerPrefs.SetString(key, value);
                }
            }
        }

        [MenuItem("itsxwz/Transform/Modify Transforms Data", false, 100)]
        public static void Apply()
        {
            var allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            var previousSelection = Selection.objects;
            Selection.objects = allObjects;
            var transforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
            Selection.objects = previousSelection;
            foreach (var trans in transforms)
            {
                string hashCode = trans.GetHashCode().ToString();
                if (PlayerPrefs.HasKey(hashCode))
                {
                    string[] datas = PlayerPrefs.GetString(hashCode).Split('/');
                    if (datas.Length == 9)
                    {
                        float.TryParse(datas[0], out float posX);
                        float.TryParse(datas[1], out float posY);
                        float.TryParse(datas[2], out float posZ);
                        Vector3 pos = new Vector3(posX, posY, posZ);

                        float.TryParse(datas[3], out float angleX);
                        float.TryParse(datas[4], out float angleY);
                        float.TryParse(datas[5], out float angleZ);
                        Vector3 angle = new Vector3(angleX, angleY, angleZ);

                        float.TryParse(datas[6], out float scaleX);
                        float.TryParse(datas[7], out float scaleY);
                        float.TryParse(datas[8], out float scaleZ);
                        Vector3 scale = new Vector3(scaleX, scaleY, scaleZ);

                        trans.localPosition = pos;
                        trans.localEulerAngles = angle;
                        trans.localScale = scale;
                    }

                    PlayerPrefs.DeleteKey(hashCode);
                }
            }
        }
    }
}