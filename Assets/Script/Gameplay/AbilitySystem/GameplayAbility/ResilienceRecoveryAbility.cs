using System.Threading.Tasks;
using JetBrains.Annotations;
using Script.Gameplay.Fx;
using Script.Gameplay.Sound;
using UnityEngine;
using Yd.Animation;
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

                Owner.Character.PlayGameplaySound(GameplaySound.Stun);
                Owner.Character.Controller.AbilitySystem.DeactivateAllOtherAbilities(this);
                Owner.Character.ShowGameplayFx(GameplayFx.Stun, true);
                Owner.Character.Animator.SetValue(AnimatorParameterId.Stun, true);
            }
            else if (resilienceRecoveryEffect is { IsCompleted: true } &&
                     resilience.CurrentValue >=
                     Owner.AttributeSet.GetAttribute(GameplayAttributeTypeEnum.MaxResilience).CurrentValue)
            {
                Debug.Log("[ResilienceRecoveryAbility::Tick] resilience = MAX");
                Owner.RemoveGameplayEffect(resilienceRecoveryEffect.Result);
                resilienceRecoveryEffect = null;
                recoverTimer = null;

                Tags.Remove("Stun");

                AllowMovement = true;
                AllowRotation = true;

                Owner.Character.ShowGameplayFx(GameplayFx.Stun, false);
                Owner.Character.Animator.SetValue(AnimatorParameterId.Stun, false);
            }
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
            Owner.Character.ShowGameplayFx(GameplayFx.Stun, false);
            Owner.Character.Animator.SetValue(AnimatorParameterId.Stun, false);
        }
    }
}