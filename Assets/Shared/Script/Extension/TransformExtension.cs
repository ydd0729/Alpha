using UnityEngine;

namespace Shared.Extension
{
    public static class TransformExtension
    {
        public static void SetPositionAndRotation(this Transform transform, Transform other)
        {
            transform.SetPositionAndRotation(other.position, other.rotation);
        }
    }

    public static class Vector3Extension
    {
        public static Vector3 Ground(this Vector3 v)
        {
            v.y = 0;
            return v;
        }
    }
}