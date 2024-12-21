using System;
using UnityEngine;

namespace Yd.Extension
{
    [ExecuteAlways]
    public class DebugDrawer : MonoBehaviour
    {
        public DebugShape Shape;
        public float Radius;
        public float Height;
        public Color Color;
        public int Segment;

        private Vector3[] wireShape;

        private void OnDrawGizmosSelected()
        {
            wireShape = Shape switch
            {
                DebugShape.WireCircularArea => WireShape.CircularArea(transform.position, Radius, Height, Segment),
                DebugShape.WireSphere => WireShape.Sphere(transform.position, Radius, Segment),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            GizmosE.DrawShape(wireShape, Color);
        }

        public void Init(DebugShape shape, float radius, float height, Color color, int segment = 0)
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