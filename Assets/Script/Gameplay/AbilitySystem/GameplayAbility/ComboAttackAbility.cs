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
    public class ComboAttackAbility : ComboAbility
    {

        private readonly HashSet<Collider> hitted = new();

        private bool damageDetection;

        public ComboAttackAbility(
            GameplayAbilityData data, GameplayAbilitySystem owner, [CanBeNull] GameplayAbilitySystem source
        ) : base(data, owner, source)
        {
            Tags.Add("Attack");
        }

        public ComboAttackAbilityData AttackAbilityData => (ComboAttackAbilityData)Data;

        protected override async Task<bool> StartExecution()
        {
            // Debug.LogWarning($"{nameof(ComboAttackAbility)} Starts.");

            if (!await base.StartExecution())
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
                Animator.SetValue(AnimatorParameterId.AttackId, Data.AttackId);
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

            // Debug.LogWarning("StopExecution");
        }

        public override void Tick()
        {
            base.Tick();

            if (damageDetection)
            {
                var damageData = AttackAbilityData.Damage[ComboCounter - 1];
                var colliders = PhysicsE.OverlapSphere
                (
                    Character.BodyParts[damageData.bindBone].transform.position,
                    damageData.radius,
                    LayerMaskE.Character,
                    QueryTriggerInteraction.Ignore,
                    true,
                    StaticColor.Get(DebugColorType.Red),
                    StaticColor.Get(DebugColorType.Blue)
                );

                foreach (var collider in colliders)
                {
                    if (collider.gameObject == Owner.OwnerCharacter.gameObject)
                    {
                        continue;
                    }

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
            var damageData = AttackAbilityData.Damage[ComboCounter - 1];
            taggedDamages.Add(key: "Health", -damageData.healthDamage.Random());
            taggedDamages.Add(key: "Resilience", -damageData.resilienceDamage.Random());

            foreach (var collider in hitted)
            {
                var character = collider.gameObject.GetComponent<Character>();
                // Debug.Log($"[ComboAttackAbility::ApplyDamage] {character.name}");

                // TODO: Ugly
                character?.Controller.NavigateTo
                (
                    character.transform.position +
                    (Owner.OwnerCharacter.transform.position - character.transform.position).normalized * 0.01f
                );

                character?.Controller?.AbilitySystem.ApplyGameplayEffectAsync
                    (AttackAbilityData.DamageEffect, Owner, taggedDamages);
            }
        }

        protected override async Task<bool> CanExecute()
        {
            if (!await base.CanExecute())
            {
                return false;
            }

            return Owner.OwnerCharacter.Movement.CurrentState == MovementState.Stand &&
                   Owner.OwnerCharacter.Weapon == CharacterWeapon.None;
        }

        private void OnGameplayEvent(GameplayEvent @event)
        {
            if (@event == Data.BindingEvent)
            {
                if (!ComboApproved && CanDetectCombo)
                {
                    ComboApproved = true;
                    ComboCounter += 1;
                    ComboWaiter.SetResult(true);
                }
                return;
            }

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
            }
        }
    }
}