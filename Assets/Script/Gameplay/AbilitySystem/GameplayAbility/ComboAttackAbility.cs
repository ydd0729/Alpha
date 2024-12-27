using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Script.Gameplay.Sound;
using UnityEngine;
using Yd.Animation;
using Yd.Extension;
using Yd.Gameplay.Object;
using Yd.PhysicsExtension;

namespace Yd.Gameplay.AbilitySystem
{
    public class ComboAttackAbility : ComboAbility
    {
        private const uint NumMaxHits = 50;
        private readonly Collider[] colliders = new Collider[NumMaxHits];
        private readonly HashSet<Collider> hit = new();

        private bool damageDetection;

        public ComboAttackAbility(
            GameplayAbilityData data, GameplayAbilitySystem owner, [CanBeNull] GameplayAbilitySystem source
        ) : base(data, owner, source)
        {
            Tags.Add("Attack");
        }

        private new ComboAttackAbilityData Data => (ComboAttackAbilityData)base.Data;

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
                Animator.SetValue(AnimatorParameterId.AttackId, Data.BindingAttackId);
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

            Debug.Log($"[{Owner.Character}] Combo Attack Ability Stopped");
            // if (Data.AlwaysCombo)
            // {
            //     Animator.SetValue(AnimatorParameterId.Attack, false);
            // }
        }

        public override void Tick()
        {
            base.Tick();

            // if (Data.AlwaysCombo)
            // {
            //     Animator.SetValue(AnimatorParameterId.Attack, true);
            //     Animator.SetValue(AnimatorParameterId.AttackId, Data.BindingAttackId);
            // }

            if (damageDetection)
            {
                if (Data.Weapon == Weapon.None)
                {
                    var index = ComboCounter - 1;

                    if (index < 0)
                    {
                        Debug.LogWarning("[ComboAttackAbility] index < 0 .");
                        return;
                    }

                    if (index >= Data.Damage.Count)
                    {
                        Debug.LogWarning("[ComboAttackAbility] index >= Data.Damage.Count .");
                        return;
                    }

                    var damageData = Data.Damage[index];

                    var count = PhysicsE.OverlapSphereNonAlloc
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

                    for (var i = 0; i < count; i++)
                    {
                        AddToHit(colliders[i]);
                    }
                }
            }
        }
        private void AddToHit(Collider collider)
        {
            if (collider.gameObject.CompareTag(Owner.Character.gameObject.tag))
            {
                return;
            }

            if (!hit.Contains(collider))
            {
                var character = collider.gameObject.GetComponent<Character>();
                // actor?.AudioManager.PlayOneShot(AudioId.PunchImpactFlesh, AudioChannel.World);
                if (character != null)
                {
                    character.PlayGameplaySound(GameplaySound.BeHit);
                }
                hit.Add(collider);
            }
        }

        private void ApplyDamage()
        {
            var taggedDamages = new Dictionary<string, float>();
            var damageData = Data.Damage[ComboCounter - 1];

            taggedDamages.Add(key: "Health", -damageData.healthDamage.Random());
            taggedDamages.Add(key: "Resilience", -damageData.resilienceDamage.Random());

            foreach (var collider in hit)
            {
                var character = collider.gameObject.GetComponent<Character>();
                // Debug.Log($"[ComboAttackAbility::ApplyDamage] {character.name}");

                // TODO: Ugly
                character?.Controller.NavigateTo
                (
                    character.transform.position +
                    (Owner.Character.transform.position - character.transform.position).normalized * 0.01f
                );

                character?.Controller?.AbilitySystem.ApplyGameplayEffectAsync(Data.DamageEffect, Owner, taggedDamages);
            }
        }

        protected override async Task<bool> CanExecute()
        {
            if (!await base.CanExecute())
            {
                return false;
            }

            if (!Data.CanMove && Owner.Character.Movement.CurrentState != MovementState.Stand)
            {
                Debug.Log("[ComboAttackAbility::CanExecute] failed because MovementState != Stand");
                return false;
            }
            
            if (Owner.Character.Weapon != Data.Weapon)
            {
                Debug.Log($"[ComboAttackAbility::CanExecute] failed because weapon != {Data.Weapon}");
                return false;
            }

            return true;
        }

        private void OnGameplayEvent(GameplayEventArgs args)
        {
            if (args.EventType == Data.BindingEventType)
            {
                TryCombo();
                return;
            }

            switch(args.EventType)
            {
                case GameplayEventType.DamageDetectionStart:
                    hit.Clear();
                    damageDetection = true;
                    if (Data.Weapon != Weapon.None)
                    {
                        var weapon = Owner.Character.WeaponObjects[Data.Weapon].GetComponent<WeaponBehaviour>();
                        weapon.Detected += AddToHit;
                        weapon.StartDetection(LayerMaskE.Character);
                    }
                    break;
                case GameplayEventType.DamageDetectionEnd:
                    damageDetection = false;
                    ApplyDamage();
                    if (Data.Weapon != Weapon.None)
                    {
                        var weapon = Owner.Character.WeaponObjects[Data.Weapon].GetComponent<WeaponBehaviour>();
                        weapon.Detected -= AddToHit;
                        weapon.EndDetection();
                    }
                    break;
                case GameplayEventType.ComboDetectionStart:
                    StartComboDetection();
                    break;
                case GameplayEventType.ComboDetectionEnd:
                    if (Data.AlwaysCombo)
                    {
                        TryCombo();
                    }

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

        private void TryCombo()
        {

            if (!ComboApproved && CanDetectCombo)
            {
                ComboApproved = true;
                ComboCounter += 1;
                ComboWaiter.SetResult(true);
            }
        }
    }
}