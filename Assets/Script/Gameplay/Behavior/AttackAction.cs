using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
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
    [SerializeReference] public BlackboardVariable<int> numAttacks;
    private Character character;

    protected override Status OnStart()
    {
        character = Agent.Value.GetComponent<Character>();
        var controller = character?.Controller;

        var attackId = Random.Range(0, numAttacks);
        switch(attackId)
        {
            case 0:
                controller?.OnGameplayEvent(GameplayEventType.Attack01);
                break;
            case 1:
                controller?.OnGameplayEvent(GameplayEventType.Attack02);
                break;
            case 2:
                controller?.OnGameplayEvent(GameplayEventType.Attack03);
                break;
        }

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