using System.Reflection;

namespace UnityFramework.Runtime
{
    public class ReflectionHelper
    {
        /// <summary>
        /// 得到类中所有的字段名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>以数组方式返回</returns>
        public static string[] GetClassFieldsNameArray<T>()
        {
            FieldInfo[] fieldInfos = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            string[] fieldNames = new string[fieldInfos.Length];
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                fieldNames[i] = fieldInfos[i].Name;
            }

            return fieldNames;
        }

        /// <summary>
        /// 得到类中所有的字段值
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>以数组方式返回</returns>
        public static string[] GetClassFieldsValueArray<T>(T t)
        {
            FieldInfo[] fieldInfos = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            string[] fieldValues = new string[fieldInfos.Length];
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                object obj = fieldInfos[i].GetValue(t);
                if (obj == null)
                {
                    fieldValues[i] = "";
                }
                else
                {
                    fieldValues[i] = obj.ToString();
                }
            }

            return fieldValues;
        }
    }
}