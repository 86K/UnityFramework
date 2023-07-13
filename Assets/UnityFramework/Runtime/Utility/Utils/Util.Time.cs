using System;
using System.Net.Http;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public static partial class Util
    {
        /// <summary>
        /// 时间相关的实用工具类
        /// </summary>
        public static class Time
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

        /// <summary>
        /// 从网络得到真实时间
        /// </summary>
        /// <param name="callback"></param>
        public static async void GetRealNetworkTime(Action<DateTime> callback)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://worldtimeapi.org/api/ip");

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        //{ "abbreviation":"CST","client_ip":"59.174.171.185","datetime":"2023-06-27T15:05:00.430899+08:00","day_of_week":2,"day_of_year":178,"dst":false,"dst_from":null,"dst_offset":0,"dst_until":null,"raw_offset":28800,"timezone":"Asia/Shanghai","unixtime":1687849500,"utc_datetime":"2023-06-27T07:05:00.430899+00:00","utc_offset":"+08:00","week_number":26}

                        // NOTICE 需要引入SimpleJSON库
                        //var rootNode = SimpleJSON.JSON.Parse(jsonResponse);
                        //if (rootNode != null)
                        //{
                        //    string time = rootNode["datetime"];
                        //    if (!string.IsNullOrEmpty(time))
                        //    {
                        //        string t = time.Substring(0, 10);
                        //        var times = t.Split("-");
                        //        int.TryParse(times[0], out int year);
                        //        int.TryParse(times[1], out int month);
                        //        int.TryParse(times[2], out int day);
                        //        DateTime networkTime = new DateTime(year, month, day, 0, 0, 0);
                        //        callback?.Invoke(networkTime);
                        //    }
                        //    else
                        //    {
                        //        callback?.Invoke(DateTime.Now);
                        //    }
                        //}
                        //else
                        //{
                        //    callback?.Invoke(DateTime.Now);
                        //}
                    }
                    else
                    {
                        Debug.Log($"> 无法获取网络时间：{response.StatusCode}");
                        callback?.Invoke(DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"> 获取网络时间失败：{ex}");
                callback?.Invoke(DateTime.Now);
            }
        }
    }
}