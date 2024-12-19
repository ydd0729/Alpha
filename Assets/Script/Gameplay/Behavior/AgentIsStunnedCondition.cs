using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Yd.Gameplay.Object;

[Serializable] [GeneratePropertyBag]
[Condition
    (name: "Agent is Stunned", story: "[Agent] is Stunned", category: "Conditions", id: "f446d7c877690f413cd3f96955147efc")]
public class AgentIsStunnedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    private string[] StunTag = { "Stun" };

    public override bool IsTrue()
    {
        return Agent.Value.GetComponent<Character>().Controller.AbilitySystem.GetActiveAbilitiesWithTags(StunTag).Count != 0;
    }
}