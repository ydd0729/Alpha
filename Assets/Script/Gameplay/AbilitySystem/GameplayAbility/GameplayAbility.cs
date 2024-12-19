using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Yd.Animation;
using Yd.Collection;
using Yd.Gameplay.Object;

namespace Yd.Gameplay.AbilitySystem
{
    public abstract class GameplayAbility
    {
        protected List<GameplayEffect> AppliedEffects = new();

        protected GameplayAbility(
            GameplayAbilityData data, GameplayAbilitySystem owner, [CanBeNull] GameplayAbilitySystem source
        )
        {
            Data = data;
            Owner = owner;
            Source = source;

            Character = Owner.OwnerCharacter;

            if (Character)
            {
                Controller = Character.Controller;
                Animator = Character.GetComponent<Animator>();
                CharacterMovement = Character.Movement;
                AnimationEventDispatcher = Character.GetComponent<AnimationEventDispatcher>();
            }
        }

        public bool IsExecuting
        {
            get;
            protected set;
        }

        public bool AllowRotation { get; protected set; }
        public bool AllowMovement { get; protected set; }

        public List<string> Tags { get; private set; } = new();

        public GameplayAbilityData Data { get; }
        public GameplayAbilitySystem Owner { get; }

        public SRangeFloat CooldownSpan { get; protected set; } = new();
        public float CooldownRemainingTime => CooldownSpan.IsInRange(Time.time) ? CooldownSpan.End - Time.time : 0;
        public bool IsCoolingDown { get; protected set; }

        protected GameplayAbilitySystem Source { get; }
        protected Character Character { get; }
        protected Animator Animator { get; private set; }
        protected GameplayCharacterController Controller { get; }
        [CanBeNull] protected PlayerCharacterController PlayerController => Controller as PlayerCharacterController;
        protected CharacterMovement CharacterMovement { get; private set; }
        protected AnimationEventDispatcher AnimationEventDispatcher { get; private set; }

        public virtual void Tick()
        {
            if (IsCoolingDown && !CooldownSpan.IsInRange(Time.time))
            {
                StopCooldown();

                if (!Data.Passive)
                {
                    Owner.DeactivateAbility(this);
                }
            }
        }

        public async Task<bool> TryExecute()
        {
            if (!await CanExecute())
            {
                Debug.Log("[GameplayAbility::CanExecute] Failed");
                return false;
            }
            Debug.Log("[GameplayAbility::CanExecute] Success");

            IsExecuting = true;

            if (!await StartExecution())
            {
                StopExecution();
                return false;
            }

            return true;
        }

        protected virtual async Task<bool> CanExecute()
        {
            return !IsExecuting &&
                   !IsCoolingDown &&
                   await TryApplyCost() &&
                   Owner.GetActiveAbilitiesWithTags(Data.ForbiddenTags).Count == 0;
        }

        private void StartCooldown(float? cooldown = null)
        {
            IsCoolingDown = true;
            CooldownSpan.Start = Time.time;
            CooldownSpan.End = CooldownSpan.Start + (cooldown ?? Data.Cooldown);
        }

        protected void StopCooldown()
        {
            IsCoolingDown = false;
        }

        protected virtual async Task<bool> StartExecution()
        {
            foreach (var effectData in Data.EffectsToApply)
            {
                var effect = await Owner.ApplyGameplayEffectAsync(effectData, Source);
                AppliedEffects.Add(effect);
            }

            var tasks = Owner.ApplyGameplayEffectAsync(Data.EffectsToApply, Source);

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                AppliedEffects.Add(await task);
            }

            return true;
        }

        public virtual void StopExecution()
        {
            IsExecuting = false;
            StartCooldown();

            // TODO 感觉放在这里不合适
            // foreach (var effect in AppliedEffects)
            // {
            //     Owner.RemoveGameplayEffect(effect);
            // }
        }

        protected async Task<bool> TryApplyCost()
        {
            if (Data.Cost == null)
            {
                return true;
            }

            var gameplayEffect = await Owner.ApplyGameplayEffectAsync(Data.Cost, Owner);
            return gameplayEffect.AllModsValid;
        }
    }
}