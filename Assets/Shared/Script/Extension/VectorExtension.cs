using UnityEngine;

namespace Shared.Extension
{
    public static class VectorExtension
    {
        public static Vector3 Ground(this Vector3 v)
        {
            v.y = 0;
            return v;
        }
        
        public static Vector3 Pow(Vector3 v, float p)
        {
            return new Vector3(Mathf.Pow(v.x, p), Mathf.Pow(v.y, p), Mathf.Pow(v.z, p));
        }

        public static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
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
    }
}