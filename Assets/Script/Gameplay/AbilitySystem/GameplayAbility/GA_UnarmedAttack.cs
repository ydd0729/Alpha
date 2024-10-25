using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.AbilitySystem
{
    public class GA_UnarmedAttack : ComboAbility
    {
        public GA_UnarmedAttack(
            GameplayAbilityData data, GameplayAbilitySystem owner, [CanBeNull] GameplayAbilitySystem source
        ) : base(data, owner, source)
        {
        }

        protected override async Task<bool> Execute()
        {
            Debug.LogWarning($"{nameof(GA_UnarmedAttack)} Starts.");

            if (!await base.Execute())
            {
                return false;
            }

            AllowMovement = false;
            AllowRotation = false;

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

        public override void StopExecution()
        {
            base.StopExecution();

            AnimationEventDispatcher.Gameplay -= OnGameplayEvent;
            if (PlayerController)
            {
                PlayerController.GameplayEvent -= OnGameplayEvent;
            }

            AllowMovement = true;
            AllowRotation = true;
        }

        protected override void EndCooldown()
        {
            base.EndCooldown();
            Owner.Deactivate(this);
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
                    if (!ComboApproved)
                    {
                        StopExecution();
                    }
                    else
                    {
                        EndComboDetection();
                    }

                    break;
                case GameplayEvent.NormalAttack:
                    if (!ComboApproved && CanDetectCombo)
                    {
                        ComboWaiter.SetResult(true);
                        ComboApproved = true;
                        ComboCounter += 1;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@event), @event, null);
            }
        }
    }
}