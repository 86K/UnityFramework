namespace UnityFramework.Runtime
{
    public static class Version
    {
        private static IVersionHelper m_VersionHelper = null;

        public static void SetVersionHelper(IVersionHelper versionHelper)
        {
            m_VersionHelper = versionHelper;
        }

        public static string ServerVerion
        {
            get
            {
                if(m_VersionHelper == null)
                {
                    return string.Empty;
                }
                return m_VersionHelper.ServerVersion;
            }
        }

        public static string GameVersion
        {
            get
            {
                if(m_VersionHelper == null)
                {
                    return string.Empty;
                }
                return m_VersionHelper.GameVersion;
            }
        }
    }
}