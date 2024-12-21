using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Yd.Extension;
using Yd.Gameplay.Object;
using Action = Unity.Behavior.Action;

[Serializable] [GeneratePropertyBag]
[NodeDescription
(
    name: "Agent Look At Position",
    story: "[Agent] Look at [Position]",
    category: "Action",
    id: "f7556c2bea9e000be4f3d6ad5f8477a0"
)]
public class AgentLookAtPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Vector3> Position;
    private GameplayCharacterController controller;

    private Vector3 lookAtDirection;

    protected override Status OnStart()
    {
        controller = Agent.Value.GetComponent<Character>().Controller;
        lookAtDirection = (Position.Value - controller.Character.transform.position).Ground().normalized;
        controller.SetTargetDirection(lookAtDirection);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var angle = Vector3.Angle(controller.Character.transform.forward, lookAtDirection);
        // Debug.Log($"[Agent Look At Position] Angle = {angle}]");
        return angle >= GameplayCharacterController.RotationTolerance
            ? Status.Running
            : Status.Success;
    }
}