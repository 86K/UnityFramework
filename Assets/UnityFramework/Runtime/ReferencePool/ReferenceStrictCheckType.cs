namespace UnityFramework.Runtime
{
    public enum ReferenceStrictCheckType : byte
    {
        AlwaysEnable = 0,
        OnlyEnableInDevelopment,
        OnlyEnableInEditor,
        AlwaysDisable
    }
}