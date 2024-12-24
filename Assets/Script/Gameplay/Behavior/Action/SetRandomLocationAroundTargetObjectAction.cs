using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Yd.Algorithm;
using Action = Unity.Behavior.Action;

[Serializable] [GeneratePropertyBag]
[NodeDescription
(
    name: "Set Random Location Around TargetObject",
    story: "Set Random Location Around [TargetObject]",
    category: "Action",
    id: "cd98c44564b61fb79111c6dde6b74a99"
)]
public class SetRandomLocationAroundTargetObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> TargetObject;
    [SerializeReference] public BlackboardVariable<float> innerRadius;
    [SerializeReference] public BlackboardVariable<float> radius;

    protected override Status OnStart()
    {
        var randomInCircle = RandomE.RandomInRing(innerRadius, radius);
        var location = TargetObject.Value.transform.position + new Vector3(randomInCircle.x, 0, randomInCircle.y);

        NavMesh.SamplePosition(location, out var navMeshHit, 20, NavMesh.AllAreas);

        if (navMeshHit.hit)
        {
            GameObject.GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue
                (name: "Random Location", location);
            return Status.Success;
        }

        return Status.Failure;
    }
}