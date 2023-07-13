namespace UnityFramework.Runtime
{
    public interface IVersionHelper
    {
        /// <summary>
        /// 服务器版本号
        /// </summary>
        string ServerVersion { get; }

        /// <summary>
        /// 游戏版本号
        /// </summary>
        string GameVersion { get; }
    }
}