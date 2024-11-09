using UnityEngine;

namespace Yd.Extension
{
    public static class GizmosE
    {
        public static void DrawSphere(Vector3 origin, float radius, Color color = default, int segment = 0)
        {
            DrawShape(WireShape.Sphere(origin, radius, segment), color);
        }

        public static void DrawCircularArea(Vector3 origin, float radius, float height, Color color = default, int segment = 0)
        {
            DrawShape(WireShape.CircularArea(origin, radius, height, segment), color);
        }

        private static void DrawShape(Vector3[] shape, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLineList(shape);
        }
    }
}