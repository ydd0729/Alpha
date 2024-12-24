using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable] [GeneratePropertyBag]
[NodeDescription
    (name: "Set Target Location", story: "Set [Target] Location", category: "Action", id: "2393a3cc6cf66de1e1dfcd814a2748db")]
public class SetTargetLocationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        GameObject.GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue
            (name: "Target Location", Target.Value.transform.position);
        return Status.Success;
    }
}