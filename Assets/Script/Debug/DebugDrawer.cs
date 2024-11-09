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
        public DebugColorType Color;
        public int Segment;

        private void OnDrawGizmos()
        {
            Draw();
        }

        public void Init(DebugShape shape, float radius, float height, DebugColorType color, int segment = 0)
        {
            Shape = shape;
            Radius = radius;
            Height = height;
            Color = color;
            Segment = segment;
        }

        private void Draw()
        {
            switch(Shape)
            {
                case DebugShape.CircularArea:
                    GizmosE.DrawCircularArea(transform.position, Radius, Height, StaticColor.Get(Color), Segment);
                    break;
                case DebugShape.Sphere:
                    GizmosE.DrawSphere(transform.position, Radius, StaticColor.Get(Color), Segment);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum DebugShape
    {
        Sphere,
        CircularArea
    }

}