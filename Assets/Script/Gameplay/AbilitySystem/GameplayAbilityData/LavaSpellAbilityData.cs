using UnityEngine;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu
        (fileName = "Lava Spell Ability", menuName = "Scriptable Objects/Gameplay/Gameplay Ability System/Lava Spell Ability")]
    public class LavaSpellAbilityData : SimpleAttackAbilityData
    {
        [SerializeField] private float spellPrepareDuration;
        [SerializeField] private float spellDuration;
        [SerializeField] private GameObject fireballPrefab;

        public float SpellPrepareDuration => spellPrepareDuration;
        public float SpellDuration => spellDuration;

        public GameObject FireballPrefab => fireballPrefab;

        public new static int BindingAttackId => 1;

        public override GameplayAbility Create(GameplayAbilitySystem owner, GameplayAbilitySystem source)
        {
            return new LavaSpellAbility(this, owner, source);
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

            return args.EventType == BindingEventType && args.AttackId == BindingAttackId;
        }
    }
}