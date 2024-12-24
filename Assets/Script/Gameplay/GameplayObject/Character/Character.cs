using System;
using System.Collections.Generic;
using System.Linq;
using Animation;
using JetBrains.Annotations;
using Script.Gameplay.Fx;
using Script.Gameplay.GameplayObject.Item;
using Script.Gameplay.Sound;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Yd.Animation;
using Yd.Audio;
using Yd.Collection;
using Yd.Extension;
using Yd.Gameplay.AbilitySystem;
using Yd.Manager;
using Action = System.Action;

namespace Yd.Gameplay.Object
{
    [RequireComponent(typeof(Animator))]
    [SelectionBase]
    public class Character : Actor
    {
        [SerializeField] private GameObject controllerPrefab;
        [FormerlySerializedAs("characterData")] [SerializeField] protected CharacterData data;
        [FormerlySerializedAs("healthBar")] [SerializeField] private StatsBar statsBar;
        [SerializeField] private SDictionary<GameplayBone, GameObject> bodyParts;
        [FormerlySerializedAs("gameplayFx")] [SerializeField] private SDictionary<GameplayFx, GameObject> gameplayVfx;
        [SerializeField] private SDictionary<GameplaySound, SKeyValuePair<AudioId, AudioChannel>> gameplaySounds;

        [FormerlySerializedAs("weaponSockets")] [SerializeField] private SDictionary<Weapon, GameObject> weaponObjects;
        private readonly List<Weapon> availableWeapons = new() { Weapon.None };

        private int? animatorLayerIndex;

        private NavMeshAgent navMeshAgent;

        private GameObject target;

        private Weapon weapon;

        public IReadOnlyDictionary<Weapon, GameObject> WeaponObjects => weaponObjects;

        public IReadOnlyList<Weapon> AvailableWeapons => availableWeapons;

        public Animator Animator { get; private set; }
        public CharacterController UnityController { get; private set; }
        public HashSet<GameObject> TriggeredObjects
        {
            get;
        } = new();

        public CharacterMovement Movement { get; private set; }

        public GameplayCharacterController Controller { get; private set; }

        public Rigidbody Rigidbody { get; private set; }

        public CharacterData Data => data;
        public PlayerCharacterData PlayerCharacterData => (PlayerCharacterData)Data;

        public AiSensor AiSensor { get; protected set; }
        [CanBeNull] public GameObject Target
        {
            get
            {
                if (AiSensor)
                {
                    target = AiSensor.Objects.Count > 0 ? AiSensor.Objects[0] : null;
                }
                return target;
            }

            set => target = value;
        }

        protected StatsBar StatsBar => statsBar;

        public BehaviorGraphAgent BehaviorGraphAgent { get; private set; }
        public Weapon Weapon
        {
            get => weapon;
            set
            {
                if (TrySetWeapon(value))
                {
                    weapon = value;
                }
            }
        }

        public bool IsGrounded => Movement.GroundDistance <=
                                  (Movement.CurrentState.IsGroundState
                                      ? Data.GroundToleranceWhenGrounded
                                      : Data.GroundToleranceWhenFalling);

        public ICollection<IInteractive> TriggeredInteractives => TriggeredObjects.Select
            (triggered => triggered.GetComponent<IInteractive>()).Where(interactive => interactive != null).ToList();

        public IReadOnlyDictionary<GameplayBone, GameObject> BodyParts => bodyParts;
        public AudioId FootstepAudioId
        {
            set;
            get;
        }
        public bool IsDead
        {
            get;
            private set;
        }
        private int AnimatorSwordLayerIndex => animatorLayerIndex ??= Animator.GetLayerIndex("Sword");

        private void Awake()
        {
            Animator = GetComponent<Animator>();

            UnityController = GetComponent<CharacterController>();
            UnityController.detectCollisions = false;

            Rigidbody = GetComponent<Rigidbody>();

            Movement = this.GetOrAddComponent<CharacterMovement>();
            Movement.Initialize(this);

            Controller = Instantiate(controllerPrefab, transform.parent).GetComponent<GameplayCharacterController>();
            // 在 Instantiate 返回前就会执行 Awake ，不是想要的，把初始化放在 Initialize 中
            Controller.Initialize(this);

            var animationEventDispatcher = GameObjectExtension.GetOrAddComponent<AnimationEventListener>(gameObject);
            animationEventDispatcher.GameplayEventDispatcher += OnGameplayEvent;
            animationEventDispatcher.AnimationEventDispatcher += OnAnimationEvent;

            AiSensor = GetComponent<AiSensor>();
            if (AiSensor)
            {
                AiSensor.Initialize(this);
            }

            BehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            if (BehaviorGraphAgent)
            {
                if (AiSensor)
                {
                    BehaviorGraphAgent.BlackboardReference.SetVariableValue(name: "Target", AiSensor.Target);
                    AiSensor.TargetChanged += o => {
                        BehaviorGraphAgent.BlackboardReference.SetVariableValue(name: "Target", o);
                    };
                }
                BehaviorGraphAgent.BlackboardReference.SetVariableValue(name: "Speed", Data.speed);
            }

            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent)
            {
                navMeshAgent.speed = Data.speed;
            }

            Weapon = Weapon.None;
        }

        private void Update()
        {
            var health = Controller.AbilitySystem.AttributeSet.GetAttribute(GameplayAttributeTypeEnum.Health).CurrentValue;
            // Debug.Log(health);

            if (!IsDead && health == 0f)
            {
                Die();
            }
        }

        private void OnAnimatorMove()
        {
            AnimatorMoved?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggeredObjects.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggeredObjects.Remove(other.gameObject);
        }

        private void OnAnimationEvent(string @event)
        {
            switch(@event)
            {
                case AnimationEventType.Step:
                    PlayGameplaySound(GameplaySound.Step);
                    break;
            }
        }

        public void ShowGameplayFx(GameplayFx fx, bool enabled)
        {
            if (gameplayVfx.ContainsKey(fx))
            {
                gameplayVfx[fx].gameObject.SetActive(enabled);
            }
        }

        public void PlayGameplaySound(GameplaySound sound)
        {
            if (sound == GameplaySound.Step)
            {
                AudioManager.PlayOneShot(FootstepAudioId, AudioChannel.Footstep);
                return;
            }

            var (id, channel) = gameplaySounds[sound];
            AudioManager.PlayOneShot(id, channel);
        }

        private void OnGameplayEvent(GameplayEventArgs args)
        {
        }

        public void SetGrounded(bool isGrounded)
        {
            if (isGrounded)
            {
                UnityController.enabled = true;
                Rigidbody.isKinematic = true;
                Rigidbody.interpolation = RigidbodyInterpolation.None;
            }
            else
            {
                UnityController.enabled = false;
                Rigidbody.isKinematic = false;
                Rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
                // Rigidbody.interpolation = RigidbodyInterpolation.None;
            }
        }

        public void OnHealthChanged(float health)
        {
            StatsBar.SetHealth(health);
        }

        public void OnMaxHealthChanged(float health)
        {
            StatsBar.SetMaxHealth(health);
        }

        public void OnResilienceChanged(float resilience)
        {
            StatsBar.SetResilience(resilience);
        }

        public void OnMaxResilienceChanged(float value)
        {
            StatsBar.SetMaxResilience(value);
        }

        public event Action AnimatorMoved;

        public void Die()
        {
            IsDead = true;

            if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = true;
            }

            if (BehaviorGraphAgent)
            {
                BehaviorGraphAgent.enabled = false;
            }

            Controller.OnDie();
            Animator.SetValue(AnimatorParameterId.Die, true);

            CoroutineTimer.SetTimer(_ => statsBar.gameObject.SetActive(false), 0.8f, CoroutineTimerLoopPolicy.Once);

            Dead?.Invoke();

            Destroy(gameObject, 10);
        }

        public event Action Dead;

        private bool TrySetWeapon(Weapon weapon)
        {
            if (weapon != Weapon.None && !availableWeapons.Contains(weapon))
            {
                return false;
            }

            switch(weapon)
            {
                case Weapon.None:
                    if (AnimatorSwordLayerIndex != -1)
                    {
                        Animator.SetLayerWeight(AnimatorSwordLayerIndex, 0);
                    }

                    if (weaponObjects.ContainsKey(Weapon.Sword))
                    {
                        weaponObjects[Weapon.Sword].SetActive(false);
                    }
                    break;
                case Weapon.Sword:
                    if (AnimatorSwordLayerIndex != -1)
                    {
                        Animator.SetLayerWeight(AnimatorSwordLayerIndex, 1f);
                    }

                    if (weaponObjects.ContainsKey(Weapon.Sword))
                    {
                        weaponObjects[Weapon.Sword].SetActive(true);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weapon), weapon, null);
            }

            return true;
        }

        public void GrantWeapon(Weapon weapon)
        {
            if (!availableWeapons.Contains(weapon))
            {
                availableWeapons.Add(weapon);
            }
        }
    }


    public enum Weapon
    {
        None,
        Sword
    }

    public enum GameplayBone
    {
        LeftFoot,
        RightFoot,
        LeftHand,
        RightHand,
        Head,
        None
    }
}