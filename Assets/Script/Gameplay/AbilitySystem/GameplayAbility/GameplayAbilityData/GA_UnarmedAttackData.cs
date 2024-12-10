using System;
using UnityEngine;
using Yd.Gameplay.Object;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu
    (
        fileName = "Unarmed Attack Ability",
        menuName = "Scriptable Objects/Gameplay/Gameplay Ability System/Unarmed Attack Ability"
    )]
    public class GA_UnarmedAttackData : ComboAbilityData
    {
        public override GameplayAbility Create(GameplayAbilitySystem owner, GameplayAbilitySystem source)
        {
            return new GA_UnarmedAttack(this, owner, source);
        }

        public override async void OnGameplayEvent(GameplayEvent type, GameplayAbilitySystem owner)
        {
            switch(type)
            {
                case GameplayEvent.NormalAttack:
                    if (owner.OwnerCharacter.Movement.CurrentState == MovementState.Stand &&
                        owner.OwnerCharacter.Weapon == CharacterWeapon.None)
                    {
                        await owner.TryActivateAbility(this);
                    }
                    break;
                case GameplayEvent.DamageDetectionStart:
                case GameplayEvent.DamageDetectionEnd:
                case GameplayEvent.ComboDetectionStart:
                case GameplayEvent.ComboDetectionEnd:
                case GameplayEvent.Interact:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}