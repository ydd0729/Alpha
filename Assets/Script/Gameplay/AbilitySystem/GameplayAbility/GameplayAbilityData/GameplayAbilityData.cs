using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Yd.Gameplay.AbilitySystem
{
    public abstract class GameplayAbilityData : ScriptableObject
    {
        [SerializeField] private int maxActivation = 1;
        [SerializeField] private float cooldown;
        [SerializeField] [CanBeNull] private GameplayEffectData cost;
        [SerializeField] private GameplayEffectData[] effectsToApply;
        [SerializeField] private bool passive;
        [SerializeField] private GameplayEvent bindingEvent;
        [SerializeField] private int attackId;
        [SerializeField] private List<string> forbiddenTags;

        public float Cooldown => cooldown;

        [CanBeNull] public GameplayEffectData Cost => cost;
        public GameplayEffectData[] EffectsToApply => effectsToApply;

        public int MaxActivation => maxActivation;
        public bool Passive => passive;

        public GameplayEvent BindingEvent => bindingEvent;
        public int AttackId => attackId;
        public IReadOnlyList<string> ForbiddenTags => forbiddenTags;

        public abstract GameplayAbility Create(GameplayAbilitySystem owner, GameplayAbilitySystem source);

        // public virtual void OnGameplayEvent(GameplayEvent type, GameplayAbilitySystem owner)
        // {
        // }
    }
}