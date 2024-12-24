using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Yd.Extension;
using Yd.Gameplay.Object;
using Action = Unity.Behavior.Action;

[Serializable] [GeneratePropertyBag]
[NodeDescription
    (name: "Look at Object", story: "[Agent] looks at [Object]", category: "Action", id: "0f36891f36f3c77f620500e4d8aa6995")]
public class LookAtObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    private GameplayCharacterController controller;

    private Vector3 lookAtDirection;

    protected override Status OnStart()
    {
        controller = Agent.Value.GetComponent<Character>().Controller;
        lookAtDirection = (Object.Value.transform.position - controller.Character.transform.position).Ground().normalized;
        controller.SetTargetDirection(lookAtDirection);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var angle = Vector3.Angle(controller.Character.transform.forward, lookAtDirection);
        // Debug.Log($"[Agent Look At Position] Angle = {angle}]");
        return angle >= GameplayCharacterController.RotationTolerance ? Status.Running : Status.Success;
    }
}