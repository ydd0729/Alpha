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
                case GameplayEvent.DamageDetectionStart:
                    break;
                case GameplayEvent.DamageDetectionEnd:
                    break;
                case GameplayEvent.ComboDetectionStart:
                    break;
                case GameplayEvent.ComboDetectionEnd:
                    break;
                case GameplayEvent.NormalAttack:
                    if (owner.OwnerCharacter.Movement.CurrentState == MovementState.Stand &&
                        owner.OwnerCharacter.Weapon == CharacterWeapon.None)
                    {
                        await owner.TryActivateAbility(this);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}