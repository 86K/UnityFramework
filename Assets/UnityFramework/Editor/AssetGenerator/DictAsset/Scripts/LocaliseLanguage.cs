using System;
using UnityEngine;

namespace UnityFramework.Editor
{
    [CreateAssetMenu(fileName = "LocaliseLanguage", menuName = "itsxwz/Asset/String - LocaliseLanguage", order = 0)]
    public class LocaliseLanguage : BaseAsset<string, LocaliseLanguage.LanguageNames>
    {
        [Serializable]
        public struct LanguageNames
        {
            public string zh_CN;
            public string en_US;
            public string fr_FR;
        }
    }
}