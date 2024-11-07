using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Yd.Extension;

namespace Yd.Gameplay.Object
{
    public class CharacterControllerBase : Actor
    {
        [FormerlySerializedAs("UseGravity")] [SerializeField]
        private bool useGravity = true;

        [FormerlySerializedAs("data")] [SerializeField] protected ControllerData controllerData;
        private readonly Vector3[] historyPosition = new Vector3[2];
        private Vector3 lastFrameVelocity;
        private RotationTask rotationTask;

        protected NavMeshAgent NavMeshAgent { get; private set; }

        public ControllerData ControllerData => controllerData;

        public Character Character { get; private set; }

        public EWalkRunToggle WalkRunToggle { get; protected set; }

        public Vector3 Velocity { get; private set; }

        public bool IsNavigating => NavMeshAgent.isOnNavMesh && !NavMeshAgent.isStopped;

        public Vector3 GroundVelocity => Velocity.Ground();

        // 角色旋转
        private Quaternion CharacterRotation
        {
            get => Character.transform.rotation;
            set => Character.transform.rotation = value;
        }

        private Quaternion CharacterTargetRotation => Quaternion.LookRotation
            (transform.TransformDirection(LocalMoveDirection).Ground());

        private Vector3 CharacterPosition => Character.transform.position;
        private float StepOffset => Character.UnityCharacterController.stepOffset;

        public Vector3 LocalMoveDirection { get; protected set; }

        private bool AllowRotation => Character.Movement.CurrentState.AllowRotation &&
                                      (AbilitySystem == null || AbilitySystem.AllowRotation);

        public bool AllowMovement => AbilitySystem.AllowMovement;

        protected virtual void Update()
        {
            if (!ControllerData.useStrafeSet && !IsNavigating && AllowRotation)
            {
                UpdateRotation();
            }

            if (!ControllerData.useRootMotionMovement)
            {
                Move();
            }
        }

        protected virtual void LateUpdate()
        {
            FollowCharacterPosition();

            historyPosition[1] = historyPosition[0];
            historyPosition[0] = CharacterPosition;
            lastFrameVelocity = (historyPosition[0] - historyPosition[1]) / Time.deltaTime;
        }

        public event Action<GameplayEvent> GameplayEvent;

        public void NavigateTo(Vector3 position)
        {
            NavMeshAgent.enabled = true;
            NavMeshAgent.destination = position;
        }

        public void OnGameplayEvent(GameplayEvent obj)
        {
            GameplayEvent?.Invoke(obj);
        }

        public virtual void Initialize(Character character)
        {
            Character = character;
            NavMeshAgent = Character.GetComponent<NavMeshAgent>();
            
            if (ControllerData.useRootMotionMovement)
            {
                Character.AnimatorMoved += Move;
            }

            Velocity = Vector3.zero;
            WalkRunToggle = EWalkRunToggle.Run;

            historyPosition[0] = CharacterPosition;
            historyPosition[1] = CharacterPosition;
            lastFrameVelocity = Vector3.zero;

            if (AbilitySystem)
            {
                AbilitySystem.Initialize(this);
            }

            FollowCharacterPosition();
        }

        private void FollowCharacterPosition()
        {
            transform.position = CharacterPosition + ControllerData.controllerFollowOffset;
        }

        private void Move()
        {
            Character.Movement.DetectGround();

            Character.UnityCharacterController.enabled = !IsNavigating;
            NavMeshAgent.enabled = IsNavigating;

            var motion = Vector3.zero;

            if (ControllerData.useRootMotionMovement)
            {
                if (IsNavigating)
                {
                    // TODO
                }
                else
                {
                    if (!Character.Movement.CurrentState.ShouldGrounded)
                    {
                        var v0 = Velocity;

                        var acceleration = (useGravity ? ControllerData.gravitationalAcceleration : Vector3.zero) +
                                           ControllerData.airDrag.Mult(Velocity.Pow(2)).Mult(-Velocity.Sign());

                        Velocity += acceleration * Time.deltaTime;
                        motion = (v0 + Velocity) * Time.deltaTime / 2;
                    }
                    else
                    {
                        Velocity = lastFrameVelocity; // 下落时使用
                        motion = Vector3.down * Character.UnityCharacterController.stepOffset;
                    }
                }
            }
            else
            {
                if (IsNavigating)
                {
                    Velocity = NavMeshAgent.velocity;
                    LocalMoveDirection = Velocity.magnitude > 0.05f ? Velocity.normalized : Vector3.zero;
                }
                // ReSharper disable once RedundantIfElseBlock
                else
                {
                    // TODO
                }
            }

            if (motion != Vector3.zero)
            {
                Character.UnityCharacterController.Move(motion);
            }
        }

        protected void Jump()
        {
            Velocity = GroundVelocity + // 避免在斜坡上起跳时的影响
                       (Character.Movement.LastState is not StandState
                           ? Character.transform.forward * ControllerData.jumpForwardSpeed
                           : Vector3.zero) + // 跳跃向前的速度
                       Vector3.up * ControllerData.jumpUpSpeed; // 跳跃向上的速度
        }

        private void UpdateRotation()
        {
            if (LocalMoveDirection != Vector3.zero && CharacterTargetRotation != rotationTask.Target)
            {
                rotationTask.SetTask(CharacterRotation, CharacterTargetRotation, controllerData.timeToRotate);
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