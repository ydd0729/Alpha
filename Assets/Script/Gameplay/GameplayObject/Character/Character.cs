using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Script.Gameplay.GameplayObject.Item;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Yd.Animation;
using Yd.Audio;
using Yd.Collection;
using Yd.Extension;
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
        [CanBeNull] public GameObject Target => AiSensor.Objects.Count > 0 ? AiSensor.Objects[0] : null;

        protected StatsBar StatsBar => statsBar;

        public BehaviorGraphAgent behaviorGraphAgent { get; private set; }

        public CharacterWeapon Weapon
        {
            get;
            protected set;
        }

        public bool IsGrounded => Movement.GroundDistance <=
                                  (Movement.CurrentState.IsGroundState
                                      ? Data.GroundToleranceWhenGrounded
                                      : Data.GroundToleranceWhenFalling);

        public ICollection<IInteractive> TriggeredInteractives => TriggeredObjects.Select
            (triggered => triggered.GetComponent<IInteractive>()).Where(interactive => interactive != null).ToList();

        public IReadOnlyDictionary<GameplayBone, GameObject> BodyParts => bodyParts;

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

            var animationEventDispatcher = GameObjectExtension.GetOrAddComponent<AnimationEventDispatcher>(gameObject);
            animationEventDispatcher.Event += OnGameplayEvent;

            AiSensor = GetComponent<AiSensor>();
            if (AiSensor)
            {
                AiSensor.Initialize(this);
            }

            behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            if (behaviorGraphAgent)
            {
                behaviorGraphAgent.BlackboardReference.SetVariableValue(name: "Target", AiSensor.Target);
                behaviorGraphAgent.BlackboardReference.SetVariableValue(name: "Speed", Data.speed);

                AiSensor.TargetChanged += o => { behaviorGraphAgent.BlackboardReference.SetVariableValue(name: "Target", o); };
            }

            var navAgent = GetComponent<NavMeshAgent>();
            if (navAgent)
            {
                navAgent.speed = Data.speed;
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

        private void OnGameplayEvent(GameplayEvent obj)
        {
            switch(obj)
            {
                case GameplayEvent.PunchSound:
                    AudioManager.PlayOneShot(AudioId.Punch, AudioChannel.World);
                    break;
                case GameplayEvent.KickSound:
                    AudioManager.PlayOneShot(AudioId.Kick, AudioChannel.World);
                    break;
            }
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

        public event Action AnimatorMoved;
    }

    public enum CharacterWeapon
    {
        None
    }

    public enum GameplayBone
    {
        LeftFoot,
        RightFoot,
        LeftHand,
        RightHand
    }


}