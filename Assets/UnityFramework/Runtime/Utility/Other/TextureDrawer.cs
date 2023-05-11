namespace UnityFramework.Runtime
{
    using UnityEngine;
    
    public class TextureDrawer
    {
        /// <summary>
        /// 高斯正态分布概率
        /// </summary>
        /// <param name="_ndc">Normal distribution coefficient 正态分布系数</param> 
        /// <param name="_start">起始点 0</param> 
        /// <param name="_end">终结点 1</param> 
        /// <returns></returns>
        static float NormalDistribution(float _ndc, float _start, float _end)
        {
            float pi = 1 / Mathf.Sqrt(2 * Mathf.PI); //0.3989423f;
            float variance = _end * _end;
            float power = -(Mathf.Pow(Mathf.Abs(_ndc - _start), 2) / (2 * variance));
            float probability = (pi / _end) * Mathf.Exp(power);
            return probability;
        }

        /// <summary>
        /// 通过高斯正态分布公式生成一张离散效果图
        /// </summary>
        /// <param name="_width">图片宽度</param> 
        /// <param name="_height">图片高度</param> 
        /// <param name="_radius">画出圆形的半径</param> 
        /// <param name="_alpha">所有画出像素点的alpha值</param> 
        /// <param name="_color">所有画出像素点的颜色值  1,0,0</param> 
        /// <param name="_restrain">离散约束 ---- 值越小画出的像素点越密集  0.05</param> 
        /// <returns></returns>
        public static Texture2D DrawCircluarMap(int _width, int _height, int _radius, float _alpha, Vector3 _color, float _restrain = 0.2f)
        {
            Texture2D tex = new Texture2D(_width, _height, TextureFormat.ARGB32, true);
            Color[] colors = new Color[_width * _height];
            int halfWidth = _width / 2;
            int halfHeight = _height / 2;

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    //根据距离进行高斯分布概率计算
                    float distribution = (Mathf.Sqrt((i - halfWidth) * (i - halfWidth) + (j - halfHeight) * (j - halfHeight))) / _radius;
                    float probability = NormalDistribution(distribution, 0, 1); ///[0~0.3989423f]

                    //设置绘制区域
                    bool discreteness = Random.Range(0, probability) > _restrain ? true : false; // _restrain的数据越大，绘制的像素点越少
                    if (discreteness)
                    {
                        colors[i * _width + j] = new Color(_color.x / 255, _color.y / 255, _color.z / 255, probability * _alpha);
                    }
                    else
                    {
                        colors[i * _width + j] = new Color(1, 1, 1, 1);
                    }
                }
            }

            tex.SetPixels(colors);
            tex.Apply();
            colors = null;
            System.GC.Collect();
            return tex;
        }

        /// <summary>
        /// 通过缩放已有的贴图，生成一张形变的贴图
        /// </summary>
        /// <param name="_tex">已有贴图</param> 
        /// <param name="_width">形变贴图的宽度</param> 
        /// <param name="_height">形变贴图的高度</param> 
        /// <param name="_scaleX">形变贴图在x上的缩放比例</param> 
        /// <param name="_scaleY">形变贴图在y上的缩放比例</param> 
        /// <returns></returns>
        public static Texture2D DrawChangeShapeMap(Texture2D _tex, int _width, int _height, float _scaleX, float _scaleY)
        {
            Texture2D tex = new Texture2D(_width, _height, TextureFormat.ARGB32, true);
            int halfWidth = _width / 2;
            int halfHeight = _height / 2;
            int scaleWidth = (int) (halfWidth * _scaleX);
            int scaleHeight = (int) (halfHeight * _scaleY);
            Color[] texColors = _tex.GetPixels(); //传入贴图的所有像素点颜色
            Color[] colors = new Color[_width * _height];

            int texX, texY; //生成贴图的x.y
            for (int i = halfHeight - scaleHeight; i < halfHeight + scaleHeight; i++)
            {
                for (int j = halfWidth - scaleWidth; j < halfWidth + scaleWidth; j++)
                {
                    texX = (int) (halfHeight + (i - halfHeight) * 1.0f / _scaleY);
                    texY = (int) (halfWidth + (j - halfWidth) * 1.0f / _scaleX);
                    texX = (texX >= 0) ? texX : 0;
                    texY = (texY >= 0) ? texY : 0;
                    texX = (texX < _height) ? texX : _height - 1;
                    texY = (texY < _width) ? texY : _width - 1;
                    colors[i * _width + j] = texColors[texX * _width + texY];
                }
            }

            tex.SetPixels(colors);
            tex.Apply();
            texColors = null;
            colors = null;
            System.GC.Collect();
            return tex;
        }
    }
}