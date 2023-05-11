namespace UnityFramework.Runtime
{
    public partial class Algorithm
    {
        /// <summary>
        /// 排序算法
        /// </summary>
        public class Sort
        {
            /// <summary>
            /// 冒泡排序  O(n^2)
            /// 重复“从序列右边开始比较相邻两个数字的大小，在根据结果交换两个数字的位置”这一操作的算法
            /// </summary>
            /// <param name="_array"></param>
            public static void BubbleSort(int[] _array)
            {
                int len = _array.Length, left = 0, right = 0, tmp = 0;
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < len - 1; j++)
                    {
                        left = len - 2 - j;
                        right = len - 1 - j;
                        if (_array[left] > _array[right])
                        {
                            tmp = _array[left];
                            _array[left] = _array[right];
                            _array[right] = tmp;
                        }
                    }
                }
            }

            /// <summary>
            /// 选择排序 O(n^2)
            /// 重复"从待排序的数据中寻找最小值，将其与序列最左边的数字进行交换"这一操作的算法
            /// </summary>
            /// <param name="_array"></param>
            public static void SelectionSort(int[] _array)
            {
                int len = _array.Length, index = 0, tmp = 0, min = 0;
                for (int i = 0; i < len - 1; i++)
                {
                    min = _array[i];
                    index = i;
                    for (int j = i + 1; j < len; j++)
                    {
                        if (min > _array[j])
                        {
                            min = _array[j];
                            index = j;
                        }
                    }

                    tmp = _array[i];
                    _array[i] = _array[index];
                    _array[index] = tmp;
                }
            }

            /// <summary>
            /// 插入排序  O(n^2)
            /// 从右侧的未排序区域取出一个数据插入到左侧已排序区域合适的位置
            /// </summary>
            /// <param name="_array"></param>
            public static void InsertionSort(int[] _array)
            {
                int len = _array.Length, tmp = 0;
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (_array[i] < _array[j])
                        {
                            tmp = _array[i];
                            _array[i] = _array[j];
                            _array[j] = tmp;
                        }
                    }
                }
            }

            /// <summary>
            /// 归并排序  O(nLogn)
            /// 重复“把序列分成长度相同的两个子序列，当无法继续往下分时(每个子序列只有一个数据时)，就对子序列进行归并”操作，直到所有子序列都归并为一个整体为止
            /// </summary>
            /// <param name="_array"></param>
            public static void MergeSort(int[] _array, int _first, int _last)
            {
                if (_first < _last)
                {
                    int mid = (_first + _last) / 2;
                    MergeSort(_array, _first, mid);
                    MergeSort(_array, mid + 1, _last);
                    Merge(_array, _first, mid, _last);
                }
            }

            static void Merge(int[] _array, int _first, int _mid, int _last)
            {
                int[] leftArr = new int[_mid - _first + 2];
                int[] rightArr = new int[_last - _mid + 1];
                leftArr[_mid - _first + 1] = int.MaxValue;
                rightArr[_last - _mid] = int.MaxValue;

                for (int i = 0; i < leftArr.Length - 1; i++)
                {
                    leftArr[i] = _array[_first + i];
                }

                for (int i = 0; i < rightArr.Length - 1; i++)
                {
                    rightArr[i] = _array[_mid + i + 1];
                }

                int pLeft = 0, pRight = 0;
                for (int i = 0; i < _last - _first + 1; i++)
                {
                    if (leftArr[pLeft] <= rightArr[pRight])
                    {
                        _array[_first + i] = leftArr[pLeft];
                        pLeft++;
                    }
                    else
                    {
                        _array[_first + i] = rightArr[pRight];
                        pRight++;
                    }
                }
            }

            /// <summary>
            /// 快速排序 O(nLogn)
            /// 在序列中随机选择一个基准值，然后分为“比基准值小的数”和“比基准值大的数”这两类，在对这两类进行快速排序知道排序完成
            /// </summary>
            public static void QuickSort(int[] _array, int _first, int _last)
            {
                if (_first >= _last)
                {
                    return;
                }

                int i = _first, j = _last;
                int value = _array[_first];
                while (i < j)
                {
                    while (i < j && _array[j] >= value)
                    {
                        j--;
                    }

                    _array[i] = _array[j];
                    while (i < j && _array[i] <= value)
                    {
                        i++;
                    }

                    _array[j] = _array[i];
                }

                _array[i] = value;
                QuickSort(_array, _first, i - 1);
                QuickSort(_array, i + 1, _last);
            }
        }
    }
}