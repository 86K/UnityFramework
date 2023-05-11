using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// [NOTE]
    /// 每一个后端定义的接口可能都不一样，需要适当调整本脚本，可作为参考
    /// </summary>
    public class HttpFileUploader
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        public static async void UploadFiles(string url, string token, string filePath)
        {
            Debug.Log("UploadFiles....");
            try
            {
                AuthenticationHeaderValue authentication = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authentication;
                MultipartFormDataContent content = new MultipartFormDataContent();
                if (!File.Exists(filePath))
                {
                    Debug.LogError($"该文件不存在:{filePath}");
                    return;
                }

                content.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "file", Path.GetFileName(filePath));
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nException Caught!");
                Debug.Log("Message :" + e.Message);
            }
        }

        public static async void DownloadFiles(string url, string fileName)
        {
            Debug.Log("DownloadFiles....");
            try
            {
                //AuthenticationHeaderValue authentication = new AuthenticationHeaderValue("Bearer", token);
                //client.DefaultRequestHeaders.Authorization = authentication;
                //HttpContent httpContent = new StringContent("");
                //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nException Caught!");
                Debug.Log("Message :" + e.Message);
            }
        }

        static void HttpRequestTest(string url)
        {
            //创建HttpWeb请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //创建HttpWeb相应
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Debug.Log("Content length is " + response.ContentLength);
            Debug.Log("Content type is " + response.ContentType);

            //获取response的流
            Stream receiveStream = response.GetResponseStream();

            //使用streamReader读取流数据
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            Debug.Log("Response stream received.");
            Debug.Log(readStream.ReadToEnd());
            response.Close();
            readStream.Close();
        }

        /// <summary>
        /// 上传json请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">postData:json格式的请求报文,例如：{"key1":"value1","key2":"value2"}</param>
        /// <returns></returns>
        static string PostUrl(string url, string postData)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 10000; //设置请求超时时间，单位为毫秒
            req.ContentType = "application/json";
            byte[] data = Encoding.UTF8.GetBytes(postData);
            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();

            //获取响应内容
            string result = "";
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        /// <summary>
        /// 上传文件请求
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="formDatas">表单数据（字典格式）</param>
        /// <param name="callback">上传回调</param>
        static void UploadRequest(string url, string filePath, Dictionary<string, string> formDatas,
            Action<string> callback)
        {
            // 时间戳，用做boundary
            string timeStamp = DateTime.Now.Ticks.ToString("x");

            //根据uri创建HttpWebRequest对象
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
            httpReq.Method = "POST";
            httpReq.AllowWriteStreamBuffering = false; //对发送的数据不使用缓存
            httpReq.Timeout = 300000; //设置获得响应的超时时间（300秒）
            httpReq.ContentType = "multipart/form-data; boundary=" + timeStamp;
            //httpReq.Headers.Add("Authorization", "");

            //读取file文件
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);

            //表单信息
            string boundary = "--" + timeStamp;
            string form = "";
            string formFormat = boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
            string formEnd = boundary +
                             "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\";\r\nContent-Type:application/octet-stream\r\n\r\n";
            foreach (var pair in formDatas)
            {
                form += string.Format(formFormat, pair.Key, pair.Value);
            }

            form += string.Format(formEnd, "file", Path.GetFileName(filePath));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(form);

            //结束边界
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + timeStamp + "--\r\n");
            long length = fileStream.Length + postHeaderBytes.Length + boundaryBytes.Length;
            httpReq.ContentLength = length; //请求内容长度

            try
            {
                //每次上传4k
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];

                //已上传的字节数
                long offset = 0;
                int size = binaryReader.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();

                //发送请求头部消息
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    size = binaryReader.Read(buffer, 0, bufferLength);
                }

                //添加尾部边界
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();

                //获取服务器端的响应
                using (HttpWebResponse response = (HttpWebResponse)httpReq.GetResponse())
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    string returnValue = readStream.ReadToEnd();
                    Debug.Log("upload result:" + returnValue);
                    callback?.Invoke(returnValue);

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.Log("文件传输异常： " + ex.Message);
            }
            finally
            {
                fileStream.Close();
                binaryReader.Close();
            }
        }
    }
}