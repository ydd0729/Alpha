using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Script.Gameplay.Sound;
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
        private const uint NumMaxHits = 50;
        private readonly HashSet<Collider> hitted = new();
        private readonly Collider[] colliders = new Collider[NumMaxHits];

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

            AnimationEventListener.GameplayEventDispatcher += OnGameplayEvent;
            if (PlayerController)
            {
                PlayerController.GameplayEvent += OnGameplayEvent;
            }

            while (await Combo())
            {
                Animator.SetValue(AnimatorParameterId.Attack, true);
                // Animator.SetValue(AnimatorParameterId.AttackId, Data.AttackId);
            }

            return true;
        }

        public override void StopExecution()
        {
            base.StopExecution();

            AnimationEventListener.GameplayEventDispatcher -= OnGameplayEvent;
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
                var index = ComboCounter - 1;

                if (index < 0)
                {
                    Debug.LogWarning("[ComboAttackAbility] index < 0 .");
                    return;
                }

                if (index >= AttackAbilityData.Damage.Count)
                {
                    Debug.LogWarning("[ComboAttackAbility] index >= AttackAbilityData.Damage.Count .");
                    return;
                }
                
                var damageData = AttackAbilityData.Damage[index];
                
                int count = PhysicsE.OverlapSphereNonAlloc
                (
                    Character.BodyParts[damageData.bindBone].transform.position,
                    damageData.radius,
                    colliders,
                    LayerMaskE.Character,
                    QueryTriggerInteraction.Ignore,
                    true,
                    StaticColor.Get(DebugColorType.Red),
                    StaticColor.Get(DebugColorType.Blue)
                );

                for (int i = 0; i < count; i++)
                {
                    if (colliders[i].gameObject == Owner.Character.gameObject)
                    {
                        continue;
                    }

                    if (colliders[i].gameObject.CompareTag(Owner.Character.gameObject.tag))
                    {
                        continue;
                    }

                    if (!hitted.Contains(colliders[i]))
                    {
                        var character = colliders[i].gameObject.GetComponent<Character>();
                        // actor?.AudioManager.PlayOneShot(AudioId.PunchImpactFlesh, AudioChannel.World);
                        character.PlayGameplaySound(GameplaySound.BeHit);
                        hitted.Add(colliders[i]);
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
                    (Owner.Character.transform.position - character.transform.position).normalized * 0.01f
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

            return Owner.Character.Movement.CurrentState == MovementState.Stand &&
                   Owner.Character.Weapon == CharacterWeapon.None;
        }

        private void OnGameplayEvent(GameplayEventType eventType)
        {
            if (eventType == Data.BindingEventType)
            {
                if (!ComboApproved && CanDetectCombo)
                {
                    ComboApproved = true;
                    ComboCounter += 1;
                    ComboWaiter.SetResult(true);
                }
                return;
            }

            switch(eventType)
            {
                case GameplayEventType.DamageDetectionStart:
                    hitted.Clear();
                    damageDetection = true;
                    break;
                case GameplayEventType.DamageDetectionEnd:
                    damageDetection = false;
                    ApplyDamage();
                    break;
                case GameplayEventType.ComboDetectionStart:
                    StartComboDetection();
                    break;
                case GameplayEventType.ComboDetectionEnd:
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