using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Yd.Animation;
using Yd.Audio;
using Yd.Extension;
using Yd.Gameplay.Object;
using Yd.PhysicsExtension;

namespace Yd.Gameplay.AbilitySystem
{
    public class GA_UnarmedAttack : ComboAbility
    {

        private readonly HashSet<Collider> hitted = new();

        private bool damageDetection;

        public GA_UnarmedAttack(
            GameplayAbilityData data, GameplayAbilitySystem owner, [CanBeNull] GameplayAbilitySystem source
        ) : base(data, owner, source)
        {
        }

        public GA_UnarmedAttackData AttackData => (GA_UnarmedAttackData)Data;

        protected override async Task<bool> Execute()
        {
            Debug.LogWarning($"{nameof(GA_UnarmedAttack)} Starts.");

            if (!await base.Execute())
            {
                return false;
            }

            AllowMovement = false;
            AllowRotation = false;

            AnimationEventDispatcher.Event += OnGameplayEvent;
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

            AnimationEventDispatcher.Event -= OnGameplayEvent;
            if (PlayerController)
            {
                PlayerController.GameplayEvent -= OnGameplayEvent;
            }

            AllowMovement = true;
            AllowRotation = true;

            Debug.LogWarning("StopExecution");
        }

        protected override void EndCooldown()
        {
            base.EndCooldown();
            Owner.Deactivate(this);
        }

        public override void Tick()
        {
            base.Tick();

            if (damageDetection)
            {
                var damageData = AttackData.Damage[ComboCounter - 1];
                var colliders = PhysicsE.OverlapSphere
                (
                    Character.BodyParts[damageData.bindBone].transform.position,
                    damageData.radius,
                    LayerMaskE.Default,
                    QueryTriggerInteraction.Ignore,
                    true,
                    StaticColor.Get(DebugColorType.Red),
                    StaticColor.Get(DebugColorType.Blue)
                );

                foreach (var collider in colliders)
                {
                    if (!hitted.Contains(collider))
                    {
                        var actor = collider.gameObject.GetComponent<Actor>();
                        actor?.AudioManager.PlayOneShot(AudioId.PunchImpactFlesh, AudioChannel.World);

                        hitted.Add(collider);
                    }
                }
            }
        }

        private void ApplyDamage()
        {
            var taggedDamages = new Dictionary<string, float>();
            var damageData = AttackData.Damage[ComboCounter - 1];
            taggedDamages.Add(key: "Health", -damageData.healthDamage.Random());
            taggedDamages.Add(key: "Resilience", -damageData.resilienceDamage.Random());

            foreach (var collider in hitted)
            {
                var character = collider.gameObject.GetComponent<Character>();
                character?.Controller?.AbilitySystem.ApplyGameplayEffectAsync(AttackData.DamageEffect, Owner, taggedDamages);
            }
        }

        private void OnGameplayEvent(GameplayEvent @event)
        {
            switch(@event)
            {
                case GameplayEvent.DamageDetectionStart:
                    hitted.Clear();
                    damageDetection = true;
                    break;
                case GameplayEvent.DamageDetectionEnd:
                    damageDetection = false;
                    ApplyDamage();
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
                        StopComboDetection();
                    }
                    break;
                case GameplayEvent.NormalAttack:
                    if (!ComboApproved && CanDetectCombo)
                    {
                        ComboApproved = true;
                        ComboCounter += 1;
                        ComboWaiter.SetResult(true);
                    }
                    break;
            }
        }
    }
}