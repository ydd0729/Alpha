using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Yd.Animation;
using Yd.Manager;

namespace Yd.Gameplay.AbilitySystem
{
    public class GA_UnarmedAttack : ComboAbility
    {
        public GA_UnarmedAttack(
            GameplayAbilityData data, GameplayAbilitySystem owner, [CanBeNull] GameplayAbilitySystem source
        ) : base(data, owner, source)
        {
        }

        protected override async Task<bool> Activate()
        {
            if (!await base.Activate())
            {
                return false;
            }

            AnimationEventDispatcher.Gameplay += OnGameplayEvent;
            if (PlayerController)
            {
                PlayerController.GameplayEvent += OnGameplayEvent;
            }

            while (await Combo())
            {
                Animator.SetValue(AnimatorParameterId.Attack, true);
            }

            return true;
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();

            AnimationEventDispatcher.Gameplay -= OnGameplayEvent;
            if (PlayerController)
            {
                PlayerController.GameplayEvent -= OnGameplayEvent;
            }
        }

        private void OnGameplayEvent(GameplayEvent @event)
        {
            switch(@event)
            {
                case GameplayEvent.DamageDetectionStart:
                    break;
                case GameplayEvent.DamageDetectionEnd:
                    break;
                case GameplayEvent.ComboDetectionStart:
                    StartComboDetection();
                    break;
                case GameplayEvent.ComboDetectionEnd:
                    EndComboDetection();
                    
                    // 等待最后一帧动画完成，TODO 改成用 GameplayEvent
                    CoroutineTimer.SetTimer(_ => Owner.DeactivateAbility(this), 1f);
                    break;
                case GameplayEvent.Move:
                    break;
                case GameplayEvent.NormalAttack:
                    if (ComboInterval && CanCombo)
                    {
                        ComboWaiter.SetResult(true);
                        ComboCounter += 1;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@event), @event, null);
            }
        }
        private void StartComboDetection()
        {
            Debug.LogWarning("ComboDetectionStarted");
            ComboInterval = true;
        }

        private void EndComboDetection()
        {
            Debug.LogWarning("ComboDetectionEnd");
            ComboInterval = false;
            if (!ComboWaiter.Task.IsCompleted)
            {
                ComboWaiter.SetResult(false);
            }
        }
    }
}