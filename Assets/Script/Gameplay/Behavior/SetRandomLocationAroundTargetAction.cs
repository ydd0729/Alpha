using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Yd.Algorithm;
using Action = Unity.Behavior.Action;

[Serializable] [GeneratePropertyBag]
[NodeDescription
(
    name: "Set Random Location Around Target",
    story: "Set Random Location Around [Target]",
    category: "Action",
    id: "d3beaf954718c6b25b25c9df4259e9ee"
)]
public class SetRandomLocationAroundTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<float> innerRadius;
    [SerializeReference] public BlackboardVariable<float> radius;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        var randomInCircle = RandomE.RandomInRing(innerRadius, radius);
        var location = Target.Value.transform.position + new Vector3(randomInCircle.x, 0, randomInCircle.y);
        GameObject.GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue(name: "Random Location", location);

        return Status.Success;
    }
}