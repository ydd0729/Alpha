using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Serialization;
using Yd.Gameplay;
using Yd.Gameplay.Object;
using Action = Unity.Behavior.Action;
using Random = UnityEngine.Random;

[Serializable] [GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[Agent] attack", category: "Action", id: "7300a4186d0d71f790715f688515e198")]
public class AttackAction : Action
{
    private static readonly string[] attackTag =
    {
        "Attack"
    };

    private static readonly string[] stunTag =
    {
        "Stun"
    };

    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [FormerlySerializedAs("numAttacksForRamdomization")] [FormerlySerializedAs("numAttacks")] [SerializeReference]
    public BlackboardVariable<int> numAttacksForRandomization;
    [SerializeReference] public BlackboardVariable<int> AttackId;

    private Character character;

    protected override Status OnStart()
    {
        character = Agent.Value.GetComponent<Character>();
        var controller = character?.Controller;

        var attackId = numAttacksForRandomization.Value != 0 ? Random.Range(0, numAttacksForRandomization) : AttackId;
        controller?.OnGameplayEvent(new GameplayAttackEventArgs { EventType = GameplayEventType.Attack, AttackId = attackId });

        return controller?.AbilitySystem.GetActiveAbilitiesWithTags(attackTag).Count != 0 ? Status.Running : Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return character.Controller.AbilitySystem.GetActiveAbilitiesWithTags(attackTag).Count != 0 &&
               character.Controller.AbilitySystem.GetActiveAbilitiesWithTags(stunTag).Count == 0
            ? Status.Running
            : Status.Success;
    }
}