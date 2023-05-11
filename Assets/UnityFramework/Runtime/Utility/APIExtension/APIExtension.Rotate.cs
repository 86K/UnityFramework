namespace UnityFramework.Runtime
{
    using UnityEngine;

    /// <summary>
    /// 旋转相关的实用工具类
    /// </summary>
    public static class APIExtension_Rotate
    {
        /// <summary>
        /// 物体绕自身旋转
        /// </summary>
        /// <param name="_self">自身物体</param>
        /// <param name="_speed">旋转速度</param>
        public static void AroundSelf(this Transform _self, float _speed)
        {
            if (_self != null)
            {
                _self.Rotate(Vector3.up, -_speed * Input.GetAxis("Mouse X"), Space.World);
                _self.Rotate(Vector3.right, _speed * Input.GetAxis("Mouse Y"), Space.World);
            }
        }
        
        /// <summary>
        /// 物体绕目标物体旋转
        /// </summary>
        /// <param name="_self">自身物体</param>
        /// <param name="_target">目标物体</param>
        /// <param name="_speed">旋转速度</param>
        public static void AroundTarget(Transform _self, Transform _target, float _speed)
        {
            if (_self != null && _target != null)
            {
                var position = _target.position;
                _self.RotateAround(position, _self.up, _speed * Input.GetAxis("Mouse X"));
                _self.RotateAround(position, _self.right, -_speed * Input.GetAxis("Mouse Y"));
            }
        }
    }
}