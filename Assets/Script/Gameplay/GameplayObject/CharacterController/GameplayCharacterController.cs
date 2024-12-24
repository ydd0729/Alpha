using System;
using UnityEngine;
using UnityEngine.AI;
using Yd.Animation;
using Yd.Extension;
using Yd.Gameplay.AbilitySystem;

namespace Yd.Gameplay.Object
{
    public class GameplayCharacterController : Actor
    {
        public const float RotationTolerance = 10f;
        private GameplayAttributeSet attributeSet;
        private RotationTask rotationTask;

        private Vector3? targetDirection;

        public NavMeshAgent NavMeshAgent { get; private set; }

        public Character Character { get; private set; }

        public bool IsRotating => rotationTask.IsExecuting;

        public EWalkRunToggle WalkRunToggle { get; protected set; }

        public bool RotateToVelocity { get; set; } = true;

        public Vector3 Velocity
        {
            get
            {
                if (IsNavigating)
                {
                    return NavMeshAgent.velocity;
                }

                if (Character.Movement.CurrentState.IsGroundState)
                {
                    return Character.UnityController.velocity;
                }

                return Character.Rigidbody.linearVelocity;
            }
        }

        public Vector3 GroundVelocity => Character.UnityController.velocity.Ground();

        public bool IsNavigating => NavMeshAgent.isOnNavMesh && (NavMeshAgent.hasPath || NavMeshAgent.pathPending);

        // 角色旋转
        private Quaternion CharacterRotation
        {
            get => Character.transform.rotation;
            set => Character.transform.rotation = value;
        }

        private Quaternion CharacterTargetRotation => Quaternion.LookRotation
            (transform.TransformDirection(LocalMoveDirection).Ground());

        private Vector3 CharacterPosition => Character.transform.position;
        private float StepOffset => Character.UnityController.stepOffset;

        public Vector3 LocalMoveDirection { get; protected set; }

        private bool AllowRotation => Character.Movement.CurrentState.AllowRotation &&
                                      (AbilitySystem == null || AbilitySystem.AllowRotation);

        public bool AllowMovement => AbilitySystem.AllowMovement;

        protected virtual void Update()
        {
            if (!Character.Data.useRootMotionMovement)
            {
                Move();
            }

            // Debug.Log($"{Character.name} allow rotation = {AllowRotation}");

            if (AllowRotation)
            {
                Rotate();
            }

            // DebugE.LogValue($"{Character.gameObject.name}::{nameof(LocalMoveDirection)}", LocalMoveDirection);
        }

        protected virtual void LateUpdate()
        {
            FollowCharacterPosition();
        }

        public event Action<GameplayEventArgs> GameplayEvent;

        public bool NavigateTo(Vector3 position, float stoppingDistance = 0.5f)
        {
            targetDirection = (position - Character.transform.position).Ground().normalized;

            if (!NavMeshAgent.isOnNavMesh || !NavMeshAgent.enabled)
            {
                return false;
            }

            if (NavMeshAgent.SetDestination(position))
            {
                NavMeshAgent.stoppingDistance = stoppingDistance;
                // Character.Animator.SetValue(AnimatorParameterId.SpeedMagnitude, Character.Data.moveAnimationSpeed);

                return true;
            }

            return false;
        }

        public void StopNavigation()
        {
            if (IsNavigating)
            {
                NavMeshAgent.ResetPath();
            }
        }

        public void OnGameplayEvent(GameplayEventArgs args)
        {
            switch(args.EventType)
            {
                case GameplayEventType.Interact:
                    foreach (var interactive in Character.TriggeredInteractives)
                    {
                        interactive.Interact(gameObject);
                    }
                    break;
            }

            GameplayEvent?.Invoke(args);
        }

        public virtual void Initialize(Character character)
        {
            Character = character;
            Character.Movement.MovementStateChanged += OnMovementStateChanged;

            NavMeshAgent = Character.GetComponent<NavMeshAgent>();
            // NavMeshAgent.autoBraking = true;
            NavMeshAgent.updateRotation = false;

            if (Character.Data.useRootMotionMovement)
            {
                Character.AnimatorMoved += Move;
            }

            WalkRunToggle = EWalkRunToggle.Run;

            if (AbilitySystem)
            {
                AbilitySystem.Initialize(this);
            }

            attributeSet = GetComponent<GameplayAttributeSet>();
            attributeSet.AttributeCurrentValueChanged += (so, oldValue, value) => {
                switch(so.Type)
                {
                    case GameplayAttributeTypeEnum.Health:
                        character.OnHealthChanged(value);
                        break;
                    case GameplayAttributeTypeEnum.MaxHealth:
                        character.OnMaxHealthChanged(value);
                        break;
                    case GameplayAttributeTypeEnum.Resilience:
                        character.OnResilienceChanged(value);
                        break;
                    case GameplayAttributeTypeEnum.MaxResilience:
                        character.OnMaxResilienceChanged(value);
                        break;
                }
            };

            FollowCharacterPosition();
            transform.forward = Character.transform.forward;
        }

        private void OnMovementStateChanged(MovementStateContext context)
        {
            switch(context.CurrentState)
            {
                case JumpState:
                {
                    var v = GroundVelocity + // 避免在斜坡上起跳时的影响
                            (Character.Movement.LastOrNextState is not StandState
                                ? Character.transform.forward * Character.Data.jumpForwardSpeed
                                : Vector3.zero) + // 跳跃向前的速度
                            Character.transform.up * Character.Data.jumpUpSpeed; // 跳跃向上的速度

                    Character.Rigidbody.AddForce(v, ForceMode.VelocityChange);
                }
                    break;
                case FallState:
                {
                    if (context.LastOrNextState is WalkState or RunState or StandState)
                    {
                        Character.Rigidbody.AddForce(GroundVelocity, ForceMode.VelocityChange);
                    }
                }
                    break;
            }
        }

        private void FollowCharacterPosition()
        {
            transform.position = CharacterPosition + Character.Data.controllerFollowOffset;
        }

        private void Move()
        {
            Character.Movement.DetectGround();

            if (this is PlayerCharacterController)
            {
                ControlledMove();
            }
            else
            {
                NavigatedMove();
            }
        }

        private void ControlledMove()
        {
            if (Character.Data.useRootMotionMovement)
            {
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                // TODO
            }

            NavMeshAgent.nextPosition = Character.transform.position;
        }

        private void NavigatedMove()
        {
            if (Character.Data.useRootMotionMovement)
            {
                // TODO
            }
            else
            {
                if (targetDirection.HasValue &&
                    Vector3.Angle(Character.transform.forward, targetDirection.Value) < RotationTolerance)
                {
                    targetDirection = null;
                }

                if (RotateToVelocity && Velocity.magnitude > 0f)
                {
                    LocalMoveDirection = Velocity.normalized;
                }
                else if (targetDirection.HasValue)
                {
                    // Debug.Log($"{Character.name} target direction {targetDirection.Value}");
                    LocalMoveDirection = targetDirection.Value;
                }
                else
                {
                    LocalMoveDirection = Vector3.zero;
                }

                // Debug.Log($"{Character.name} LocalMoveDirection {LocalMoveDirection}");
                Character.Movement.CurrentState.OnTick(ref Character.Movement.context);

                Character.Animator.SetValue
                (
                    AnimatorParameterId.SpeedMagnitude,
                    Mathf.Max(Velocity.magnitude, rotationTask.IsExecuting ? Character.Data.speed : 0)
                );
            }
        }

        public void SetTargetDirection(Vector3 dir)
        {
            targetDirection = dir;
        }

        public void LookAt(Vector3 target)
        {
            SetTargetDirection(target - Character.transform.position);
        }

        private void Rotate()
        {
            // Debug.Log($"{Character.name} Rotate");

            if (LocalMoveDirection != Vector3.zero && CharacterTargetRotation != rotationTask.Target)
            {
                rotationTask.SetTask(CharacterRotation, CharacterTargetRotation, Character.Data.timeToRotate);
            }

            if (rotationTask.IsExecuting)
            {
                CharacterRotation = rotationTask.Execute();
            }
        }

        public virtual void OnDie()
        {
            AbilitySystem.DeactivateAllOtherAbilities(null);
            Destroy(gameObject);
        }
    }

    public enum EWalkRunToggle
    {
        Walk,
        Run
    }
}