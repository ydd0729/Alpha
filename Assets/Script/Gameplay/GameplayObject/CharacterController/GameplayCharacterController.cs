using System;
using UnityEngine;
using UnityEngine.AI;
using Yd.Extension;
using Yd.Gameplay.AbilitySystem;

namespace Yd.Gameplay.Object
{
    public class GameplayCharacterController : Actor
    {

        private GameplayAttributeSet attributeSet;
        private RotationTask rotationTask;

        protected NavMeshAgent NavMeshAgent { get; private set; }

        public Character Character { get; private set; }

        public EWalkRunToggle WalkRunToggle { get; protected set; }

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
            if (!Character.Data.useStrafeSet && !IsNavigating && AllowRotation)
            {
                UpdateRotation();
            }

            if (!Character.Data.useRootMotionMovement)
            {
                Move();
            }

            // DebugE.LogValue(nameof(LocalMoveDirection), LocalMoveDirection);
        }

        protected virtual void LateUpdate()
        {
            FollowCharacterPosition();
        }

        public event Action<GameplayEvent> GameplayEvent;

        public bool NavigateTo(Vector3 position, float stoppingDistance = 0.5f)
        {
            if (NavMeshAgent.SetDestination(position))
            {
                NavMeshAgent.stoppingDistance = stoppingDistance;

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

        public void OnGameplayEvent(GameplayEvent obj)
        {
            GameplayEvent?.Invoke(obj);
        }

        public virtual void Initialize(Character character)
        {
            Character = character;

            Character.Movement.MovementStateChanged += OnMovementStateChanged;
            NavMeshAgent = Character.GetComponent<NavMeshAgent>();

            if (Character.Data.useRootMotionMovement)
            {
                Character.AnimatorMoved += Move;
                NavMeshAgent.updatePosition = false;
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };

            FollowCharacterPosition();
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
        }

        private void NavigatedMove()
        {
            if (Character.Data.useRootMotionMovement)
            {
                // TODO
            }
            else
            {
                LocalMoveDirection = Velocity.magnitude > 0.05f ? Velocity.normalized : Vector3.zero;
            }
        }

        private void UpdateRotation()
        {
            if (LocalMoveDirection != Vector3.zero && CharacterTargetRotation != rotationTask.Target)
            {
                rotationTask.SetTask(CharacterRotation, CharacterTargetRotation, Character.Data.timeToRotate);
            }

            if (rotationTask.Executing)
            {
                CharacterRotation = rotationTask.Execute();
            }
        }
    }

    public enum EWalkRunToggle
    {
        Walk,
        Run
    }
}