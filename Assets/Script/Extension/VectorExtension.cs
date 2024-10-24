using UnityEngine;

namespace Yd.Extension
{
    public static class Vector3E
    {
        public static Vector3 Ground(this Vector3 v)
        {
            v.y = 0;
            return v;
        }

        public static Vector3 Mult(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3 Pow(this Vector3 v, float p)
        {
            return new Vector3(Mathf.Pow(v.x, p), Mathf.Pow(v.y, p), Mathf.Pow(v.z, p));
        }

        public static Vector3 Clamp(Vector3 v, Vector3 a, Vector3 b)
        {
            return new Vector3(
                Mathf.Clamp(v.x, a.x, b.x),
                Mathf.Clamp(v.y, a.y, b.y),
                Mathf.Clamp(v.z, a.z, b.z)
            );
        }

        public static Vector3 AutoClamp(Vector3 v, Vector3 a, Vector3 b)
        {
            var min = Vector3.Min(a, b);
            var max = Vector3.Max(a, b);

            return Clamp(v, min, max);
        }

        public static Vector3 Sign(this Vector3 v)
        {
            return new Vector3(
                Mathf.Sign(v.x),
                Mathf.Sign(v.y),
                Mathf.Sign(v.z)
            );
        }
    }
}