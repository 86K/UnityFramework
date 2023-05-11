using UnityEngine;

namespace UnityFramework.Runtime
{
    public static class APIExtension_GameObject
    {
        /// <summary>
        /// 獲取或添加組件
        /// </summary>
        /// <param name="_self"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>T（組件）</returns>
        public static T GetOrAddComponent<T>(this GameObject _self) where T : Component
        {
            T cpt = _self.GetComponent<T>();
            return cpt ? cpt : _self.AddComponent<T>();
        }
        
        // 查找所有子对象包含隐藏的
    }
}