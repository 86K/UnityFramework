using System.Collections;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public static class APIExtension_Text
    {
        /// <summary>
        /// 打字机效果输出文字内容，一个文字接一个
        /// </summary>
        /// <param name="_text">文本组件</param>
        /// <param name="_content">文字内容</param>
        /// <param name="_intervalTime">间隔时间</param>
        /// <returns></returns>
        public static IEnumerator TypeWriter(this UnityEngine.UI.Text _text, string _content, float _intervalTime)
        {
            _text.text = "";
            int index = 0;
            while (index < _content.Length)
            {
                yield return new WaitForSeconds(_intervalTime);
                _text.text += _content[index];
                index++;
            }
        }
    }
}