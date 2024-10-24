using UnityEngine;

namespace Yd.Extension
{
    public static class PhysicsE
    {
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, bool drawDebug = false, Color hitColor = default, Color unhitColor = default)
        {
            var hit = Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask);

            if (drawDebug)
            {
                Debug.DrawRay(origin, direction * (hit ? hitInfo.distance : maxDistance), hit ? hitColor : unhitColor);
            }

            return hit;
        }

        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, bool drawDebug = false, Color hitColor = default, Color unhitColor = default, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            var hit = Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, Mathf.Infinity, layerMask, queryTriggerInteraction);

            if (drawDebug)
            {
                Debug.LogWarning("can't draw capsule");
            }

            return hit;
        }

        public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, bool drawDebug = false, Color hitColor = default, Color unhitColor = default, int segment = 8, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            var hit = Physics.SphereCast(origin, radius, direction, out hitInfo, Mathf.Infinity, layerMask, queryTriggerInteraction);

            if (drawDebug)
            {
                DebugE.DrawSphere(hitInfo.point, radius, hit ? hitColor : unhitColor, segment);
            }

            return hit;
        }
    }
}