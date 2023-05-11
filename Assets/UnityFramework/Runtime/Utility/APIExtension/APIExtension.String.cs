using System.Text.RegularExpressions;

namespace UnityFramework.Runtime
{
    public static class APIExtension_String
    {
        /// <summary>
        /// 把駝峰法則的字符串分割開
        /// CamelCaseFunction -> Camel Case Function
        /// </summary>
        /// <param name="_camelCaseString"></param>
        /// <returns></returns>
        public static string SplitCamelCase(this string _camelCaseString)
        {
            if (string.IsNullOrEmpty(_camelCaseString)) return _camelCaseString;

            string camelCase = Regex.Replace(Regex.Replace(_camelCaseString, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
            string firstLetter = camelCase.Substring(0, 1).ToUpper();

            if (_camelCaseString.Length > 1)
            {
                string rest = camelCase.Substring(1);

                return firstLetter + rest;
            }

            return firstLetter;
        }
    }
}