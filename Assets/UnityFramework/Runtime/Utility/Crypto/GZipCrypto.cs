using System;
using System.IO;
using System.IO.Compression; //GZipStream
using System.Text;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// Use GZip stream to encrypt or decrypt string or bytes.
    /// </summary>
    public class GZipCrypto
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public static string EncryptString(string _string)
        {
            var sourceBytes = Encoding.GetEncoding("UTF-8").GetBytes(_string);
            var targetBytes = EncryptBytes(sourceBytes);
            string encryptStr = Convert.ToBase64String(targetBytes);
            return encryptStr;
        }
        
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public static string DecryptString(string _string)
        {
            var sourceBytes = Convert.FromBase64String(_string);
            var targetBytes = DecryptBytes(sourceBytes);
            string decryptStr = Encoding.GetEncoding("UTF-8").GetString(targetBytes);
            return decryptStr;
        }
        
        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="_bytes"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] EncryptBytes(byte[] _bytes)
        {
            try
            {
                var ms = new MemoryStream();
                var zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(_bytes, 0, _bytes.Length);
                zip.Close();
                var buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="_bytes"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] DecryptBytes(byte[] _bytes)
        {
            try
            {
                var ms = new MemoryStream(_bytes);
                var zip = new GZipStream(ms, CompressionMode.Decompress, true);
                var msreader = new MemoryStream();
                var buffer = new byte[0x1000];
                while (true)
                {
                    var reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }

                    msreader.Write(buffer, 0, reader);
                }

                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}