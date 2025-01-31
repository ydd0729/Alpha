using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Yd.Manager;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public class GameplayEffect
    {
        private readonly GameplayAbilitySystem owner;

        [CanBeNull] private readonly GameplayAbilitySystem source;

        private Dictionary<string, float> taggedValues;

        private Coroutine timer;
        public GameplayEffect(GameplayEffectData data, GameplayAbilitySystem source, GameplayAbilitySystem owner)
        {
            Data = data;
            this.source = source;
            this.owner = owner;
            taggedValues = new();
        }

        public bool AllModsValid { get; private set; }

        public bool Removed { private get; set; }

        public GameplayEffectData Data
        {
            get;
        }

        public void SetTaggedValues(Dictionary<string, float> taggedValues)
        {
            this.taggedValues = taggedValues;
        }

        public event Action AttributesDirty;

        public void OnRemove()
        {
            CoroutineTimer.Cancel(ref timer);
            AttributesDirty?.Invoke();
        }

        public bool CalculateModifications(
            IDictionary<GameplayAttributeTypeSO, float> moddingAttributes, bool useBaseValue = false
        )
        {
            IReadOnlyDictionary<GameplayAttributeTypeSO, float> sourceAttributeValues = null;
            if (source != null)
            {
                sourceAttributeValues = useBaseValue ? source.AttributeSet.BaseValues : source.AttributeSet.CurrentValues;
            }

            var targetAttributeValues = useBaseValue ? owner.AttributeSet.BaseValues : owner.AttributeSet.CurrentValues;

            foreach (var attributeModifier in Data.AttributeModifiers)
            {
                attributeModifier.Calculate(sourceAttributeValues, targetAttributeValues, moddingAttributes, taggedValues);
            }

            foreach (var (type, value) in moddingAttributes)
            {
                if (!type.Range.IsInRange(value))
                {
                    if (Data.AllModsMustValid)
                    {
                        moddingAttributes.Clear();
                    }
                    return false;
                }
            }

            return true;
        }

        public bool CheckAllModsValid()
        {
            return Data.Type switch
            {
                GameplayEffectType.Instant => CalculateModifications(new Dictionary<GameplayAttributeTypeSO, float>()),
                GameplayEffectType.Infinite => true,
                GameplayEffectType.Duration => true,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Apply()
        {
            if (Data.AllModsMustValid && !CheckAllModsValid())
            {
                AllModsValid = false;
            }
            else
            {
                AllModsValid = true;
            }

            switch(Data.Type)
            {
                case GameplayEffectType.Instant:
                    ApplyInstantEffect();
                    break;
                case GameplayEffectType.Infinite:
                    if (Data.Period == 0)
                    {
                        ApplyInfiniteEffect();
                    }
                    else
                    {
                        ApplyPeriodicEffect();
                    }
                    break;
                case GameplayEffectType.Duration:
                    if (Data.Period == 0)
                    {
                        ApplyDurationEffect();
                    }
                    else
                    {
                        ApplyPeriodicEffect();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyDurationEffect()
        {
            owner.ActiveEffects.GetValueOrAdd(Data.Channel).Add(this);
            // AttributesDirty?.Invoke();
            timer = CoroutineTimer.SetTimer(_ => owner.RemoveGameplayEffect(this), Data.Duration);
        }

        private void ApplyInfiniteEffect()
        {
            owner.ActiveEffects.GetValueOrAdd(Data.Channel).Add(this);
            // AttributesDirty?.Invoke();
        }

        private void ApplyPeriodicEffect()
        {
            owner.ActivePeriodicEffects.Add(this);

            timer = CoroutineTimer.SetTimer
            (
                context => {
                    var mods = new Dictionary<GameplayAttributeTypeSO, float>();
                    CalculateModifications(mods, true);

                    foreach (var (type, mod) in mods)
                    {
                        owner.AttributeSet[type].BaseValue = type.Range.Clamp(mod);
                    }

                    AttributesDirty?.Invoke();

                    if (Data.Type != GameplayEffectType.Infinite && context.LoopPolicy.RemainingLoopCount == 0 || Removed)
                    {
                        CoroutineTimer.Cancel(ref timer);
                        owner.RemoveGameplayEffect(this);
                    }
                },
                Data.Period,
                new CoroutineTimerLoopPolicy
                {
                    isInfiniteLoop = Data.Type == GameplayEffectType.Infinite,
                    invokeImmediately = true,
                    loopCount = (int)Math.Ceiling(Data.Duration / Data.Period)
                }
            );
        }

        private void ApplyInstantEffect()
        {
            var mods = new Dictionary<GameplayAttributeTypeSO, float>();
            CalculateModifications(mods, true);

            foreach (var (type, mod) in mods)
            {
                owner.AttributeSet[type].BaseValue = type.Range.Clamp(mod);
            }

            AttributesDirty?.Invoke();
        }
    }
}