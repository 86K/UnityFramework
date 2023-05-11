using System;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public partial class Util
    {
        public class Timestamp
        {
            //DateTime类型转换为时间戳(毫秒值)
            private static long DateToTicks(DateTime? time)
            {
                return ((time.HasValue ? time.Value.Ticks : DateTime.Parse("1990-01-01").Ticks) - 621355968000000000) /
                       10000;
            }

            /// <summary>
            /// 得到当前时间的时间戳（毫秒级别）
            /// </summary>
            /// <returns></returns>
            public static string GetTimeStamp()
            {
                return DateToTicks(DateTime.Now).ToString();
            }

            //时间戳(毫秒值)String转换为DateTime类型转换
            private static DateTime TicksToDate(string time)
            {
                long.TryParse(time, out long lt);
                long l = lt * 10000 + 621355968000000000;
                return new DateTime(l);
            }

            /// <summary>
            /// 毫秒时间戳转换为特定格式的字符串
            /// </summary>
            /// <param name="timestamp"></param>
            /// <param name="format"></param>
            /// <returns></returns>
            public static string TString(string timestamp, string format = "yyyy-MM-dd HH:mm:ss")
            {
                // NOTE：时间戳是1970的不要输出！
                if (string.IsNullOrEmpty(timestamp))
                {
                    return "";
                }

                DateTime dateTime = TicksToDate(timestamp);
                return dateTime.ToString(format);
            }
        }
    }
}