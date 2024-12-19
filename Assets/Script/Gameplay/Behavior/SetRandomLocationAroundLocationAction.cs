using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Yd.Algorithm;
using Action = Unity.Behavior.Action;

[Serializable] [GeneratePropertyBag]
[NodeDescription
(
    name: "Set Random Location Around Location",
    story: "Set Random Location Around [Location]",
    category: "Action",
    id: "27c59f311f8e5b45a3dd814c3084a088"
)]
public class SetRandomLocationAroundLocationAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Location;
    [SerializeReference] public BlackboardVariable<float> innerRadius;
    [SerializeReference] public BlackboardVariable<float> radius;

    protected override Status OnStart()
    {
        var randomInCircle = RandomE.RandomInRing(innerRadius, radius);
        var location = Location.Value + new Vector3(randomInCircle.x, 0, randomInCircle.y);
        GameObject.GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue(name: "Random Location", location);

        return Status.Success;
    }
}