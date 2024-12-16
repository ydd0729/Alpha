using UnityEngine;
using Yd.DebugExtension;

namespace Yd.PhysicsExtension
{
    public static class PhysicsE
    {
        public static bool Raycast(
            Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, bool drawDebug = false,
            Color hitColor = default, Color missColor = default
        )
        {
            var hit = Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask);

            if (drawDebug)
            {
                Debug.DrawRay(origin, direction * (hit ? hitInfo.distance : maxDistance), hit ? hitColor : missColor);
            }

            return hit;
        }

        public static bool CapsuleCast(
            Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, bool drawDebug = false,
            Color hitColor = default, Color missColor = default, float maxDistance = Mathf.Infinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
        )
        {
            var hit = Physics.CapsuleCast
                (point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);

            if (drawDebug)
            {
                Debug.LogWarning("can't draw capsule");
            }

            return hit;
        }

        public static bool SphereCast(
            Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, bool drawDebug = false,
            Color hitColor = default, Color missColor = default, int segment = 0
        )
        {
            var hit = Physics.SphereCast
                (origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);

            if (drawDebug)
            {
                DebugE.DrawSphere(hitInfo.point, radius, hit ? hitColor : missColor, segment);
            }

            return hit;
        }

        public static Collider[] OverlapSphere(
            Vector3 position, float radius, int layerMask = Physics.AllLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, bool drawDebug = false,
            Color hitColor = default, Color missColor = default, int segment = 0
        )
        {
            var colliders = Physics.OverlapSphere(position, radius, layerMask, queryTriggerInteraction);

            if (drawDebug)
            {
                DebugE.DrawSphere(position, radius, colliders.Length != 0 ? hitColor : missColor, segment);
            }

            return colliders;
        }

        public static int OverlapSphereNonAlloc(
            Vector3 position, float radius, Collider[] results, int layerMask = Physics.AllLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, bool drawDebug = false,
            Color hitColor = default, Color missColor = default, int segment = 0
        )
        {
            var count = Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, queryTriggerInteraction);

            if (drawDebug)
            {
                DebugE.DrawSphere(position, radius, count != 0 ? hitColor : missColor, segment);
            }

            return count;
        }
    }

    public static partial class LayerMaskE
    {
        public static readonly int Default = 1 << LayerMask.NameToLayer("Default");
        public static readonly int TransparentFx = 1 << LayerMask.NameToLayer("TransparentFX");
        public static readonly int IgnoreRaycast = 1 << LayerMask.NameToLayer("Ignore Raycast");
        public static readonly int Water = 1 << LayerMask.NameToLayer("Water");
        public static readonly int UI = 1 << LayerMask.NameToLayer("UI");
    }

    // user-defined
    public static partial class LayerMaskE
    {
        public static readonly int Character = 1 << LayerMask.NameToLayer("Character");
    }
}