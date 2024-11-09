using System;
using UnityEngine;
using Yd.Extension;

namespace Yd.Gameplay.Behavior
{
    [RequireComponent(typeof(DebugDrawer))]
    public class PatrolArea : MonoBehaviour
    {
        public PatrolAreaType Type;
        public float Radius;
        public Vector3 Center => transform.position;

        private void OnValidate()
        {
            var debugDrawer = gameObject.GetComponent<DebugDrawer>();

            debugDrawer.Init
            (
                Type switch
                {
                    PatrolAreaType.Circle => DebugShape.CircularArea,
                    _ => throw new ArgumentOutOfRangeException()
                },
                Radius,
                0.6f,
                DebugColorType.Green
            );
        }
    }
}