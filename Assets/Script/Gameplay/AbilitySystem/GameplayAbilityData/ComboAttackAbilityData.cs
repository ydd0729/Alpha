using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private bool alwaysCombo;
        [SerializeField] private Weapon weapon;
        [FormerlySerializedAs("attackId")] [SerializeField] private int bindingAttackId;

        public IReadOnlyList<AttributeDamage> Damage => damage;
        public GameplayEffectData DamageEffect => damageEffect;
        public bool AlwaysCombo => alwaysCombo;
        public int BindingAttackId => bindingAttackId;
        public Weapon Weapon => weapon;

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

        public override bool CheckActivationEvent(GameplayEventArgs eventArgs)
        {
            if (!base.CheckActivationEvent(eventArgs))
            {
                return false;
            }

            var args = eventArgs as GameplayAttackEventArgs;
            if (args == null)
            {
                return false;
            }

            return args.EventType == BindingEventType && args.AttackId == bindingAttackId;
        }
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