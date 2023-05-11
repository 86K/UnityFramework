namespace UnityFramework.Runtime
{
    using System.IO;
    using UnityEngine;

    /// <summary>
    /// Only can load raw image of jpg or png.
    /// </summary>
    public class ImageHelper
    {
        static byte[] GetBytes(string _filePath)
        {
            if (!File.Exists(_filePath))
            {
                Debug.Log($"File '{_filePath}' is not exist.");
                return null;
            }
            
            FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int) fileStream.Length);
            fileStream.Close();
            fileStream.Dispose();
            return bytes;
        }
        
        public static Texture2D GetTexture(string _filePath, int _width = 1920, int _height = 1080)
        {
            byte[] bytes = GetBytes(_filePath);
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            Texture2D tex = new Texture2D(_width, _height);
            tex.LoadImage(bytes);

            string[] strs = _filePath.Split('/');
            tex.name = strs[strs.Length - 1].Split('.')[0];
            return tex;
        }

        public static Sprite GetSprite(string _filePath, int _width = 1920, int _height = 1080)
        {
            Texture2D tex = GetTexture(_filePath, _width, _height);
            if (tex == null)
            {
                return null;
            }

            Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sp.name = tex.name;
            return sp;
        }

        /// <summary>
        /// Get all texture2D accroding to the file path at the same folder level.
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_pictureType"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <returns></returns>
        public static Texture2D[] GetTextures(string _filePath, string _pictureType, int _width = 1920, int _height = 1080)
        {
            string[] files = Directory.GetFiles(_filePath, _pictureType, SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                return null;
            }
            Texture2D[] texs = new Texture2D[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                texs[i] = GetTexture(files[i], _width, _height);
                string[] strs = files[i].Split('/');
                texs[i].name = strs[strs.Length - 1].Split('.')[0];
            }

            return texs;
        }

        /// <summary>
        /// Get all sprite according to the file path at the same folder level.
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_pictureType"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <returns></returns>
        public static Sprite[] GetSprites(string _filePath, string _pictureType, int _width = 1920, int _height = 1080)
        {
            Texture2D[] texs = GetTextures(_filePath, _pictureType, _width, _height);
            if (texs == null)
            {
                return null;
            }

            Sprite[] sprites = new Sprite[texs.Length];
            for (int i = 0; i < texs.Length; i++)
            {
                sprites[i] = Sprite.Create(texs[i], new Rect(0, 0, texs[i].width, texs[i].height), new Vector2(0.5f, 0.5f));
                sprites[i].name = texs[i].name;
            }

            return sprites;
        }
    }
}