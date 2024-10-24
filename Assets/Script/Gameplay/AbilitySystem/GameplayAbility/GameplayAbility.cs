using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Yd.Animation;
using Yd.Gameplay.Object;
using Yd.Manager;

namespace Yd.Gameplay.AbilitySystem
{
    public abstract class GameplayAbility
    {
        protected List<GameplayEffect> AppliedEffects = new();
        private Coroutine cooldownTimer;
        private bool isCoolingDown;
        private float lastActivateTime;

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

        public bool IsActive
        {
            get;
            protected set;
        }

        public virtual bool AllowRotation => false;
        public virtual bool AllowMovement => false;

        public GameplayAbilityData Data { get; }
        public GameplayAbilitySystem Owner { get; }

        protected GameplayAbilitySystem Source { get; }
        public float CooldownTime => Data.Cooldown - (Time.time - lastActivateTime);
        protected Character Character { get; }
        protected Animator Animator { get; private set; }
        protected CharacterControllerBase Controller { get; }
        [CanBeNull] protected PlayerCharacterController PlayerController => Controller as PlayerCharacterController;
        protected CharacterMovement CharacterMovement { get; private set; }
        protected AnimationEventDispatcher AnimationEventDispatcher { get; private set; }

        public async Task<bool> OnActivate()
        {
            if (!await CanActivate())
            {
                Debug.LogError("Activation Failed");
                return false;
            }

            IsActive = true;
            StartCooldownTimer();

            if (!await Activate())
            {
                Owner.DeactivateAbility(this);
                return false;
            }

            return true;
        }

        protected virtual async Task<bool> CanActivate()
        {
            return !IsActive && !isCoolingDown && await TryApplyCost();
        }

        protected virtual async Task<bool> Activate()
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

        public virtual void OnDeactivate()
        {
            IsActive = false;

            foreach (var effect in AppliedEffects)
            {
                Owner.RemoveGameplayEffect(effect);
            }
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

        protected void StartCooldownTimer()
        {
            isCoolingDown = true;
            lastActivateTime = Time.time;
            CoroutineTimer.Cancel(ref cooldownTimer);
            cooldownTimer = CoroutineTimer.SetTimer(_ => { isCoolingDown = false; }, Data.Cooldown);
        }
    }
}