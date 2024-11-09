using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Yd.Gameplay.Object;

namespace Yd.Gameplay.AbilitySystem
{
    public class GameplayAbilitySystem : MonoBehaviour
    {
        private readonly HashSet<GameplayAbility> AbilitiesWaitingForRemoval = new();
        public readonly HashSet<GameplayEffect> ActivePeriodicEffects = new();
        private readonly Dictionary<GEChannel, HashSet<TaskCompletionSource<GameplayEffect>>> gameplayEffectsToApply = new();
        private readonly HashSet<GameplayAbilityData> grantedAbilities = new();
        public Action<GameplayEvent, GameplayAbilitySystem> GameplayEvent;

        private GEChannel MaxChannelToApply;

        private bool shouldUpdateAttributes;
        public SortedDictionary<GEChannel, HashSet<GameplayEffect>> ActiveEffects { get; } = new();

        private Dictionary<GameplayAbilityData, HashSet<GameplayAbility>> ActiveAbilities { get; } = new();

        public GameplayAttributeSet AttributeSet { get; private set; }

        public Actor Owner { get; protected set; }
        public GameplayCharacterController OnwerController => Owner as GameplayCharacterController;
        public Character OwnerCharacter => OnwerController.Character;

        public bool AllowRotation
        {
            get
            {
                foreach (var (_, abilities) in ActiveAbilities)
                {
                    if (abilities.Any(ability => !ability.AllowRotation))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool AllowMovement
        {
            get
            {
                foreach (var (_, abilities) in ActiveAbilities)
                {
                    if (abilities.Any(ability => !ability.AllowMovement))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private void Awake()
        {
            AttributeSet = gameObject.GetComponent<GameplayAttributeSet>();
        }

        private void Update()
        {
            foreach (var (_, abilities) in ActiveAbilities)
            {
                foreach (var ability in abilities)
                {
                    ability.Tick();
                }
            }
        }

        private void LateUpdate()
        {
            if (gameplayEffectsToApply.Count != 0)
            {
                ApplyGameplayEffect(MaxChannelToApply);
                MaxChannelToApply = GEChannel.Channel0;
            }

            if (shouldUpdateAttributes)
            {
                shouldUpdateAttributes = false;
                AttributeSet.ResetCurrentValues();
                UpdateAttributes();
            }

            if (AbilitiesWaitingForRemoval.Count != 0)
            {
                foreach (var ability in AbilitiesWaitingForRemoval)
                {
                    ActiveAbilities[ability.Data].Remove(ability);
                }

                AbilitiesWaitingForRemoval.Clear();
            }
        }

        public Task<GameplayEffect> ApplyGameplayEffectAsync(GameplayEffectData effectData, GameplayAbilitySystem source)
        {
            var gameplayEffect = new GameplayEffect(effectData, source, this);
            gameplayEffect.AttributesDirty += UpdateAttributesDelayed;

            MaxChannelToApply = (GEChannel)Math.Max((int)MaxChannelToApply, (int)effectData.Channel);

            var tcs = new TaskCompletionSource<GameplayEffect>(gameplayEffect);
            gameplayEffectsToApply.GetValueOrAdd(effectData.Channel).Add(tcs);

            return tcs.Task;
        }

        public List<Task<GameplayEffect>> ApplyGameplayEffectAsync(
            IEnumerable<GameplayEffectData> effectData, GameplayAbilitySystem source
        )
        {
            return effectData.Select(data => ApplyGameplayEffectAsync(data, source)).ToList();
        }

        private void ApplyGameplayEffect(GEChannel targetChannel = GEChannel.MAX)
        {
            AttributeSet.ResetCurrentValues();

            for (var i = 0; i < (int)targetChannel + 1; ++i)
            {
                var channel = (GEChannel)i;

                if (gameplayEffectsToApply.TryGetValue(channel, out var tcss))
                {
                    foreach (var tcs in tcss)
                    {
                        var gameplayEffect = (GameplayEffect)tcs.Task.AsyncState;
                        gameplayEffect.Apply();
                        tcs.SetResult(gameplayEffect);
                    }
                }

                UpdateAttributes(channel, i == (int)targetChannel);
            }

            gameplayEffectsToApply.Clear();
        }

        public void RemoveGameplayEffect(GameplayEffect gameplayEffect)
        {
            if (gameplayEffect.Data.IsPeriodic)
            {
                ActivePeriodicEffects.Remove(gameplayEffect);
                return;
            }

            if (ActiveEffects.TryGetValue(gameplayEffect.Data.Channel, out var gameplayEffects))
            {
                if (gameplayEffects.Contains(gameplayEffect))
                {
                    gameplayEffects.Remove(gameplayEffect);
                    gameplayEffect.OnRemove();
                }
            }
        }

        private void UpdateAttributesDelayed()
        {
            shouldUpdateAttributes = true;
        }

        private void UpdateAttributes(GEChannel start = GEChannel.Channel0, bool updateRemaining = true)
        {
            for (var channel = (int)start; channel < (int)GEChannel.MAX; ++channel)
            {
                Dictionary<GameplayAttributeType, float> mods = new();

                if (ActiveEffects.TryGetValue((GEChannel)channel, out var gameplayEffectSet))
                {
                    foreach (var gameplayEffect in gameplayEffectSet)
                    {
                        gameplayEffect.CalculateModifications(mods);
                    }
                }

                foreach (var (type, mod) in mods)
                {
                    AttributeSet[type].CurrentValue = type.Range.Clamp(mod);
                }

                if (!updateRemaining)
                {
                    break;
                }
            }
        }

        public void GrantAbility([CanBeNull] GameplayAbilityData AbilityData)
        {
            if (AbilityData == null)
            {
                return;
            }

            grantedAbilities.Add(AbilityData);
        }

        public async Task<bool> TryActivateAbility(
            GameplayAbilityData abilityData, [CanBeNull] GameplayAbilitySystem source = null
        )
        {
            if (!grantedAbilities.Contains(abilityData))
            {
                Debug.LogWarning
                (
                    $"The ability {abilityData} has not been granted to the controller of {OwnerCharacter} before activation!"
                );
                return false;
            }

            if (ActiveAbilities.GetValueOrAdd(abilityData).Count >= abilityData.MaxActivation)
            {
                return false;
            }

            var ability = abilityData.Create(this, source);

            ActiveAbilities[abilityData].Add(ability);

            return await ability.TryExecute();
        }

        public void Deactivate(GameplayAbility ability)
        {
            if (ability.Owner != this)
            {
                return;
            }

            if (!ActiveAbilities.TryGetValue(ability.Data, out var activeAbilities))
            {
                return;
            }

            if (!activeAbilities.Contains(ability))
            {
                return;
            }

            if (ability.IsExecuting)
            {
                ability.StopExecution();
            }

            AbilitiesWaitingForRemoval.Add(ability);
        }

        public void Initialize(Actor owner)
        {
            Owner = owner;

            if (OnwerController)
            {
                OnwerController.GameplayEvent += OnGameplayEvent;
            }
        }

        private void OnGameplayEvent(GameplayEvent type)
        {
            foreach (var abilityData in grantedAbilities)
            {
                abilityData.OnGameplayEvent(type, this);
            }
        }
    }
}