using UnityEngine;

namespace UnityFramework.Runtime
{
    public partial class Util
    {
        /// <summary>
        /// 时间相关的实用工具类
        /// </summary>
        public class Time
        {
            /// <summary>
            /// 得到时钟时间
            /// </summary>
            /// <param name="num">传入一个以秒做单位的“时长”</param>
            /// <param name="split">分隔符，默认是 : </param>
            /// <returns>小时:分钟:秒</returns>
            public static string GetClockTime(int num, char split = ':') {
                string clock = "00:00:00";
                if (num < 0)
                {
                    Debug.LogError($"传入的时长必须大于0！");
                    return clock;
                }
                
                
                int hour = num / 3600;
                int minute = num / 60 - hour * 60;
                int second = num % 60;

                clock = hour < 10 ? "0" + hour : hour.ToString() + split;
                clock += minute >= 0 && minute < 10 ? "0" + minute : minute.ToString() + split;
                clock += second >= 0 && second < 10 ? "0" + second : second.ToString();

                return clock;
            }
        }
    }
}