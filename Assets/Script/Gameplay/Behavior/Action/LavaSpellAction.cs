using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Yd.Gameplay;
using Yd.Gameplay.AbilitySystem;
using Yd.Gameplay.Object;
using Action = Unity.Behavior.Action;

[Serializable] [GeneratePropertyBag]
[NodeDescription
    (name: "Lava Spell", story: "[Agent] launches Lava Spell", category: "Action", id: "d6b01054336626dc98ce6d116d80399a")]
public class LavaSpellAction : Action
{
    private static readonly string[] tag =
    {
        "LavaSpell"
    };
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    private Character character;
    protected override Status OnStart()
    {
        character = Agent.Value.GetComponent<Character>();
        var controller = character?.Controller;

        controller?.OnGameplayEvent
        (
            new GameplayAttackEventArgs
                { EventType = GameplayEventType.Attack, AttackId = LavaSpellAbilityData.BindingAttackId }
        );

        return controller?.AbilitySystem.GetActiveAbilitiesWithTags(tag).Count != 0 ? Status.Running : Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return character.Controller.AbilitySystem.GetActiveAbilitiesWithTags(tag).Count != 0 ? Status.Running : Status.Success;
    }
}