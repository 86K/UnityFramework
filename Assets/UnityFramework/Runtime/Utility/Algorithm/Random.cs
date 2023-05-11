namespace UnityFramework.Runtime
{
    using UnityEngine;

    public partial class Algorithm
    {
        /// <summary>
        /// 随机算法
        /// </summary>
        public static class Random
        {
            /// <summary>
            /// 洗牌算法
            /// </summary>
            /// <param name="_array"></param>
            public static void Shuffle(int[] _array)
            {
                if (_array == null)
                {
                    Debug.LogError($"Array is NULL");
                    return;
                }

                int len = _array.Length;
                if (len == 0)
                {
                    Debug.Log($"Array's length is 0");
                    return;
                }

                int temp = 0;
                System.Random r = new System.Random();
                for (int i = 0; i < len; i++)
                {
                    int idx = r.Next(len - i);
                    temp = _array[idx];
                    _array[idx] = _array[i];
                    _array[i] = temp;
                }
            }

            /// 均等随机
            /// 权重随机
            /// <summary>
            /// 得到一个随机数组
            /// </summary>
            /// <param name="_startIndex">左侧范围值</param>
            /// <param name="_endIndex">右侧范围值</param>
            /// <returns></returns>
            public static int[] GetRandomArray(int _startIndex, int _endIndex)
            {
                if (_startIndex >= _endIndex || _startIndex <= 0 || _endIndex <= 0)
                {
                    Debug.Log("Parameter is wrong.");
                    return null;
                }

                int[] array = new int[_endIndex - _startIndex];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = UnityEngine.Random.Range(_startIndex, _endIndex);
                }

                return array;
            }
        }
    }
}