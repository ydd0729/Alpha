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
}