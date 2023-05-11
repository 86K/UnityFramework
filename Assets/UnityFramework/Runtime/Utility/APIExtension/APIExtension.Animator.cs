using UnityEngine;

namespace UnityFramework.Runtime
{
    public static class APIExtension_Animator
    {
        /// <summary>
        /// 动画是否播放完毕
        /// </summary>
        /// <param name="_animator"></param>
        /// <param name="_motionName"></param>
        /// <param name="_time">动画归一化的时间，默认是1，但是有些动画不能达到1，可以用0.95左右的值去判断</param>
        /// <returns></returns>
        public static bool AnimatorIsOvered(Animator _animator, string _motionName, float _time = 1.0f)
        {
            AnimatorStateInfo animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName(_motionName))
            {
                if (animatorStateInfo.normalizedTime >= _time)
                {
                    _animator.speed = 0;
                    return true;
                }
            }
            else
            {
                Debug.LogError($"动画：{_animator} 不包含：{_motionName}");
            }

            return false;
        }
    }
}