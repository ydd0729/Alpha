using UnityEngine;

namespace Shared.Extension
{
    public static class MathfExtension
    {
        public static float AutoClamp(float v, float a, float b)
        {
            float min = Mathf.Min(a, b);
            float max = Mathf.Max(a, b);
            return Mathf.Clamp(v, min, max);
        }

        public static Vector3 AutoClamp(Vector3 v, Vector3 a, Vector3 b)
        {
            Vector3 min = Vector3.Min(a, b);
            Vector3 max = Vector3.Max(a, b);

            return new Vector3(
                Mathf.Clamp(v.x, min.x, max.x),
                Mathf.Clamp(v.y, min.y, max.y),
                Mathf.Clamp(v.z, min.z, max.z)
            );
        }

        public static Vector3 Sign(Vector3 v)
        {
            return new Vector3(
                Mathf.Sign(v.x),
                Mathf.Sign(v.y),
                Mathf.Sign(v.z)
            );
        }
    }
}