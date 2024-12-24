using System;
using System.Linq;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Yd.Navigation;
using Action = Unity.Behavior.Action;

[Serializable] [GeneratePropertyBag]
[NodeDescription
(
    name: "Better Navigate To Target",
    "Navigates a GameObject towards another GameObject using NavMeshAgent." +
    "\nIf NavMeshAgent is not available on the [Agent] or its children, moves the Agent using its transform.",
    story: "[Agent] navigates to [Target]",
    category: "Action/Navigation",
    id: "3e7ba48782d1d767518a9b528a58fc62"
)]
public class BetterNavigateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed = new(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new("SpeedMagnitude");

    // This will only be used in movement without a navigation agent.
    [SerializeReference] public BlackboardVariable<float> SlowDownDistance = new(1.0f);
    private Animator m_Animator;
    private Vector3 m_ColliderAdjustedTargetPosition;
    private float m_ColliderOffset;
    private Vector3 m_LastTargetPosition;

    private NavMeshAgent m_NavMeshAgent;
    private float m_PreviousStoppingDistance;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        // Check if the target position has changed.
        var boolUpdateTargetPosition = !Mathf.Approximately(m_LastTargetPosition.x, Target.Value.transform.position.x) ||
                                       !Mathf.Approximately(m_LastTargetPosition.y, Target.Value.transform.position.y) ||
                                       !Mathf.Approximately(m_LastTargetPosition.z, Target.Value.transform.position.z);
        if (boolUpdateTargetPosition)
        {
            m_LastTargetPosition = Target.Value.transform.position;
            m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();
        }

        var distance = GetDistanceXZ();
        if (distance <= DistanceThreshold + m_ColliderOffset)
        {
            return Status.Success;
        }

        if (m_NavMeshAgent != null)
        {
            if (boolUpdateTargetPosition)
            {
                m_NavMeshAgent.SetDestination(m_ColliderAdjustedTargetPosition);
            }

            if (m_NavMeshAgent.IsNavigationComplete())
            {
                return Status.Success;
            }
        }
        else
        {
            float speed = Speed;

            if (SlowDownDistance > 0.0f && distance < SlowDownDistance)
            {
                var ratio = distance / SlowDownDistance;
                speed = Mathf.Max(0.1f, Speed * ratio);
            }

            var agentPosition = Agent.Value.transform.position;
            var toDestination = m_ColliderAdjustedTargetPosition - agentPosition;
            toDestination.y = 0.0f;
            toDestination.Normalize();
            agentPosition += toDestination * (speed * Time.deltaTime);
            Agent.Value.transform.position = agentPosition;

            // Look at the target.
            Agent.Value.transform.forward = toDestination;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, 0);
        }

        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.stoppingDistance = m_PreviousStoppingDistance;
        }

        m_NavMeshAgent = null;
        m_Animator = null;
    }

    protected override void OnDeserialize()
    {
        Initialize();
    }

    private Status Initialize()
    {
        m_LastTargetPosition = Target.Value.transform.position;
        m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();

        // Add the extents of the colliders to the stopping distance.
        m_ColliderOffset = 0.0f;
        var agentCollider = Agent.Value.GetComponentInChildren<Collider>();
        if (agentCollider != null)
        {
            var colliderExtents = agentCollider.bounds.extents;
            m_ColliderOffset += Mathf.Max(colliderExtents.x, colliderExtents.z);
        }

        var distance = GetDistanceXZ();
        // Debug.Log(distance);
        if (distance <= DistanceThreshold + m_ColliderOffset)
        {
            return Status.Success;
        }

        // If using animator, set speed parameter.
        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, Speed);
        }

        // If using a navigation mesh, set target position for navigation mesh agent.
        m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.speed = Speed;
            m_PreviousStoppingDistance = m_NavMeshAgent.stoppingDistance;

            m_NavMeshAgent.stoppingDistance = DistanceThreshold + m_ColliderOffset;
            m_NavMeshAgent.SetDestination(m_ColliderAdjustedTargetPosition);
        }

        return Status.Running;
    }


    private Vector3 GetPositionColliderAdjusted()
    {
        var colliders = Target.Value.GetComponentsInChildren<Collider>();
        var targetCollider = colliders.First(collider => collider.enabled);
        if (targetCollider != null)
        {
            return targetCollider.ClosestPoint(Agent.Value.transform.position);
        }
        return Target.Value.transform.position;
    }

    private float GetDistanceXZ()
    {
        var agentPosition = new Vector3
            (Agent.Value.transform.position.x, m_ColliderAdjustedTargetPosition.y, Agent.Value.transform.position.z);
        return Vector3.Distance(agentPosition, m_ColliderAdjustedTargetPosition);
    }
}