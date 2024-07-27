using UnityEditor.Graphs;
using UnityEngine;

namespace Shared.Extension
{
    public static class PhysicsExtension
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
    }
}