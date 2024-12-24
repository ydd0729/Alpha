using UnityEngine.AI;

namespace Yd.Navigation
{
    public static class NavMeshAgentExtension
    {
        internal static bool IsNavigationComplete(this NavMeshAgent agent)
        {
            return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
        }
    }
}