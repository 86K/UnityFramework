namespace UnityFramework.Runtime
{
    public interface IVersionHelper
    {
        /// <summary>
        /// �������汾��
        /// </summary>
        string ServerVersion { get; }

        /// <summary>
        /// ��Ϸ�汾��
        /// </summary>
        string GameVersion { get; }
    }
}