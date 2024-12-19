using System;
using System.Collections.Generic;
using UnityEngine;
using Yd.Collection;
using Yd.Gameplay.Object;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu
    (
        fileName = "Combo Attack Ability",
        menuName = "Scriptable Objects/Gameplay/Gameplay Ability System/Combo Attack Ability"
    )]
    public class ComboAttackAbilityData : ComboAbilityData
    {
        [SerializeField] private List<AttributeDamage> damage;
        [SerializeField] private GameplayEffectData damageEffect;

        public IReadOnlyList<AttributeDamage> Damage => damage;
        public GameplayEffectData DamageEffect => damageEffect;

        private void OnValidate()
        {
            while (damage.Count < MaxCombo)
            {
                damage.Add(new AttributeDamage());
            }
            while (damage.Count > MaxCombo)
            {
                damage.RemoveAt(damage.Count - 1);
            }
        }

        public override GameplayAbility Create(GameplayAbilitySystem owner, GameplayAbilitySystem source)
        {
            return new ComboAttackAbility(this, owner, source);
        }

        // public override async void OnGameplayEvent(GameplayEvent type, GameplayAbilitySystem owner)
        // {
        //     switch(type)
        //     {
        //         case GameplayEvent.NormalAttack:
        //             if (owner.OwnerCharacter.Movement.CurrentState == MovementState.Stand &&
        //                 owner.OwnerCharacter.Weapon == CharacterWeapon.None)
        //             {
        //                 await owner.TryActivateAbility(this);
        //             }
        //             break;
        //     }
        // }
    }

    [Serializable]
    public struct AttributeDamage
    {
        public SRangeFloat healthDamage;
        public SRangeFloat resilienceDamage;
        public GameplayBone bindBone;
        public float radius;
        public int maxHit;
    }
}