using UnityEngine;

namespace Yd.Extension
{
    public static class TransformE
    {
        public static void SetPositionAndRotation(this Transform transform, Transform other)
        {
            transform.SetPositionAndRotation(other.position, other.rotation);
        }

        public static void SetLocalPositionAndRotation(this Transform transform, Transform other)
        {
            transform.localPosition = other.localPosition;
            transform.localRotation = other.localRotation;
        }
    }
}