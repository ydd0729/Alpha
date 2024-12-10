using System;
using UnityEngine;

namespace Yd.Extension
{
    public class DebugDrawer : MonoBehaviour
    {
        public DebugShape Shape;
        public float Radius;
        public float Height;
        public DebugColorType Color;
        public int Segment;

        private Vector3[] wireShape;

        private void OnDrawGizmos()
        {
            GizmosE.DrawShape(wireShape, StaticColor.Get(Color));

            Gizmos.DrawFrustum(Vector3.zero, 90, 10, 1, 1);
        }

        private void OnValidate()
        {
            wireShape = Shape switch
            {
                DebugShape.WireCircularArea => WireShape.CircularArea(transform.position, Radius, Height, Segment),
                DebugShape.WireSphere => WireShape.Sphere(transform.position, Radius, Segment),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Init(DebugShape shape, float radius, float height, DebugColorType color, int segment = 0)
        {
            Shape = shape;
            Radius = radius;
            Height = height;
            Color = color;
            Segment = segment;
        }
    }

    public enum DebugShape
    {
        WireSphere,
        WireCircularArea
    }

}