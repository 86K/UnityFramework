using UnityEngine;

public static partial class Util
{
    public static class CameraCullingMask
    {
        public static void AddCullingMask(Camera camera, int layerIndex)
        {
            int cullingMask = camera.cullingMask | (1 << layerIndex);
            camera.cullingMask = cullingMask;
        }

        public static void AddCullingMask(Camera camera, string layerName)
        {
            int index = LayerMask.NameToLayer(layerName);
            int cullingMask = camera.cullingMask | (1 << index);
            camera.cullingMask = cullingMask;
        }

        public static void DeleteCullingMask(Camera camera, int layerIndex)
        {
            int cullingMask = camera.cullingMask & ~(1 << layerIndex);
            camera.cullingMask = cullingMask;
        }

        public static void DeleteCullingMask(Camera camera, string layerName)
        {
            int index = LayerMask.NameToLayer(layerName);
            int cullingMask = camera.cullingMask & ~(1 << index);
            camera.cullingMask = cullingMask;
        }
    }
}