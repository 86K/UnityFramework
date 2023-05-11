using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public static class APIExtension_Material
    {
        /// <summary>
        /// 得到物体上所有的材质球
        /// </summary>
        /// <param name="_self"></param>
        public static Dictionary<string, Material[]> GetAllMaterials(this Transform _self)
        {
            if (_self != null)
            {
                Renderer[] renderers = _self.GetComponentsInChildren<Renderer>();
                Dictionary<string, Material[]> materials = new Dictionary<string, Material[]>();
                foreach (Renderer renderer in renderers)
                {
                    string key = renderer.name;
                    Material[] mats = renderer.materials;
                    if (!materials.ContainsKey(key))
                    {
                        materials.Add(key, mats);
                    }
                }

                return materials;
            }

            return null;
        }

        /// <summary>
        /// 把物体上的所有材质球改为同一个
        /// </summary>
        /// <param name="_self"></param>
        /// <param name="_material"></param>
        public static void ModifyAllMaterials(this Transform _self, Material _material)
        {
            if (_self == null || _material == null)
            {
                Debug.Log("The GameObject or Material is NULL");
                return;
            }

            Renderer[] renderers = _self.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] materials = new Material[renderers[i].materials.Length];
                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j] = _material;
                }

                renderers[i].materials = materials;
            }
        }

        /// <summary>
        /// 把物体上的所有材质球修改为设置的材质球
        /// </summary>
        /// <param name="_self"></param>
        /// <param name="_materials"></param>
        public static void ModifyAllMaterials(this Transform _self, Material[][] _materials)
        {
            if (_self == null || _materials.Length == 0)
            {
                Debug.Log("The GameObject or Material is NULL");
                return;
            }

            Renderer[] renderers = _self.GetComponentsInChildren<Renderer>();
            if (_materials.Length != renderers.Length)
            {
                Debug.Log("The count of material is NOT true");
                return;
            }

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].materials.Length == _materials[i].Length)
                {
                    renderers[i].materials = _materials[i];
                }
                else
                {
                    Debug.Log($"物体：{renderers[i].gameObject.name}替换材质球失败");
                }
            }
        }
    }
}