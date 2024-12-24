using System;
using UnityEngine;
using Yd.Extension;

namespace Yd.Gameplay.Behavior
{
    [ExecuteAlways]
    [RequireComponent(typeof(DebugDrawer))]
    public class GameplayArea : MonoBehaviour
    {
        public GameplayAreaType Type;
        public float Radius;
        public Color scopeColor = Color.gray;
        public Color centerColor = Color.red;
        public Vector3 Center => transform.position;

        private DebugDrawer centerDrawer;
        private DebugDrawer scopeDrawer;

        private void GetDrawers()
        {
            var debugDrawers = gameObject.GetComponents<DebugDrawer>();
            scopeDrawer = debugDrawers[0];
            centerDrawer = debugDrawers[1];
        }

        private DebugDrawer CenterDrawer
        {
            get
            {
                if (centerDrawer == null)
                {
                    GetDrawers();
                }
                
                return centerDrawer;
            }
        }
        
        private DebugDrawer ScopeDrawer
        {
            get
            {
                if (scopeDrawer == null)
                {
                    GetDrawers();
                }
                
                return scopeDrawer;
            }
        }

        private void Awake()
        {
            CenterDrawer.Init
            (
                DebugShape.WireSphere,
                0.4f,
                0,
                centerColor,
                8
            );
        }

        private void OnValidate()
        {
            ScopeDrawer.Init
            (
                Type switch
                {
                    GameplayAreaType.Circle => DebugShape.WireCircularArea,
                    _ => throw new ArgumentOutOfRangeException()
                },
                Radius,
                0.6f,
                scopeColor
            );
            
        }
    }
}