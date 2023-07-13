namespace UnityFramework.Runtime
{
    internal abstract class UnityFrameworkModule
    {
        internal virtual int Priority => 0;

        internal abstract void Update(float logicTime, float realTime);

        internal abstract void Shutdown();
    }
}