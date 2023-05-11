using UnityEngine;

namespace UnityFramework.Runtime
{
    public static class Scaler
    {
        /// <summary>
        /// 通过鼠标滚轮里滑和外滑的方式来调整摄像机的视角达到放大或缩小场景的目的
        /// </summary>
        /// <param name="_Camera">摄像机</param>
        public static void ScaleScene(Camera _Camera)
        {
            //里滑 -> 放大
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (_Camera.fieldOfView <= 100)
                {
                    _Camera.fieldOfView += 2;
                }
                else if (_Camera.orthographicSize <= 20)
                {
                    _Camera.orthographicSize += 0.5f;
                }
            }

            //外滑 -> 缩小
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (_Camera.fieldOfView > 25)
                {
                    _Camera.fieldOfView -= 2;
                }
                else if (_Camera.orthographicSize >= 1)
                {
                    _Camera.orthographicSize -= 0.5f;
                }
            }
        }
        
        /// <summary>
        /// 通过鼠标滚轮里滑和外滑的方式来调整物体的localScale达到放大和缩小物体的目的
        /// </summary>
        /// <param name="_gameObject">要缩放的物体</param>
        /// <param name="_speed">速率</param>
        /// <param name="_minScale">最小缩放比例限制</param>
        /// <param name="_maxScale">最大缩放比例限制</param>
        public static void ScaleGameObject(GameObject _gameObject, float _speed, float _minScale = 0.3f, float _maxScale = 5f)
        {
            if (_speed <= 0)
            {
                _speed = 1;
            }

            float s = Input.GetAxis("Mouse ScrollWheel");
            float scale = 0.0f;

            if (s > 0)
            {
                scale = s * _speed;
            }

            if (s < 0)
            {
                scale = (1) / (s * _speed);
            }

            if (_gameObject.transform.localScale.x > _maxScale)
            {
                _gameObject.transform.localScale = new Vector3(_maxScale, _maxScale, _maxScale);
            }

            if (_gameObject.transform.localScale.x <= _maxScale && _gameObject.transform.localScale.x >= _minScale)
            {
                _gameObject.transform.localScale += new Vector3(scale, scale, scale);
            }

            if (_gameObject.transform.localScale.x < _minScale)
            {
                _gameObject.transform.localScale = new Vector3(_minScale, _minScale, _minScale);
            }
        }
    }
}