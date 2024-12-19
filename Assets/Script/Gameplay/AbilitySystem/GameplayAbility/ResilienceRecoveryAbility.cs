using System.Threading.Tasks;
using JetBrains.Annotations;
using Script.Gameplay.Fx;
using UnityEngine;
using Yd.Animation;
using Yd.Audio;
using Yd.Manager;

namespace Yd.Gameplay.AbilitySystem
{
    public class ResilienceRecoveryAbility : GameplayAbility
    {
        private readonly GameplayAttribute resilience;
        private Coroutine recoverTimer;

        private Task<GameplayEffect> resilienceRecoveryEffect;
        public ResilienceRecoveryAbility(
            GameplayAbilityData data, GameplayAbilitySystem owner, [CanBeNull] GameplayAbilitySystem source
        ) : base(data, owner, source)
        {
            resilience = Owner.AttributeSet.GetAttribute(GameplayAttributeTypeEnum.Resilience);
            AllowMovement = true;
            AllowRotation = true;
        }
        public new ResilienceRecoveryAbilityData Data => (ResilienceRecoveryAbilityData)base.Data;

        protected override Task<bool> StartExecution()
        {
            return base.StartExecution();
        }

        public override void StopExecution()
        {
            base.StopExecution();
        }

        public override void Tick()
        {
            base.Tick();

            // Debug.Log
            // (
            //     $"{resilience.CurrentValue} {resilience.AttributeDefinition.Range.MaxInclusive} {resilience.CurrentValue >= resilience.AttributeDefinition.Range.MaxInclusive} {resilienceRecoveryEffect}"
            // );

            if (resilience.CurrentValue == 0 && recoverTimer == null)
            {
                Debug.Log("[ResilienceRecoveryAbility::Tick] resilience = 0");
                recoverTimer = CoroutineTimer.SetTimer
                (
                    _ => { resilienceRecoveryEffect = Owner.ApplyGameplayEffectAsync(Data.RecoverEffect, Owner); },
                    Data.RecoverDelay
                );
                Tags.Add("Stun");

                AllowMovement = false;
                AllowRotation = false;

                Owner.OwnerCharacter.AudioManager.PlayOneShot(AudioId.BoarRoar, AudioChannel.World);
                Owner.OwnerCharacter.Controller.AbilitySystem.DeactivateAllOtherAbilities(this);
                Owner.OwnerCharacter.ShowGameplayFx(GameplayFx.Stun, true);
                Owner.OwnerCharacter.Animator.SetValue(AnimatorParameterId.Stun, true);
            }
            else if (resilienceRecoveryEffect is { IsCompleted: true } &&
                     resilience.CurrentValue >=
                     resilience.AttributeDefinition.Range.MaxInclusive) // TODO 这里没有考虑有 Max Attribute 的情况
            {
                Debug.Log("[ResilienceRecoveryAbility::Tick] resilience = MAX");
                Owner.RemoveGameplayEffect(resilienceRecoveryEffect.Result);
                resilienceRecoveryEffect = null;
                recoverTimer = null;

                Tags.Remove("Stun");

                AllowMovement = true;
                AllowRotation = true;

                Owner.OwnerCharacter.ShowGameplayFx(GameplayFx.Stun, false);
                Owner.OwnerCharacter.Animator.SetValue(AnimatorParameterId.Stun, false);
            }
        }
    }
}