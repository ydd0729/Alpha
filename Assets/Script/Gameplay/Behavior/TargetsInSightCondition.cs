using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Yd.Gameplay.Object;

[Serializable] [GeneratePropertyBag]
[Condition
(
    name: "TargetInSight",
    story: "Target in [Agent] 's Sight",
    category: "Conditions",
    id: "5d3f097596cbc02425d18d7b13a95d3b"
)]
public class TargetInSightCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    private Character character;

    public override bool IsTrue()
    {
        return character.targets.Count > 0;
    }

    public override void OnStart()
    {
        character = GameObject.GetComponent<Character>();
    }

    public override void OnEnd()
    {
    }
}