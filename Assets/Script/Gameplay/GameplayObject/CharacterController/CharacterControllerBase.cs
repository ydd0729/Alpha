using System;
using UnityEngine;
using UnityEngine.Serialization;
using Yd.Extension;

namespace Yd.Gameplay.Object
{
    public class CharacterControllerBase : Actor
    {
        [FormerlySerializedAs("UseGravity")] [SerializeField]
        private bool useGravity = true;

        [SerializeField] protected ControllerData data;
        private readonly Vector3[] historyPosition = new Vector3[2];
        private Vector3 lastFrameVelocity;
        private RotationTask rotationTask;
        public ControllerData Data => data;

        public Character Character { get; private set; }

        public EWalkRunToggle WalkRunToggle { get; protected set; }

        public Vector3 Velocity { get; private set; }
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
            if (AllowRotation)
            {
                UpdateRotation();
            }

            Move();
        }

        // protected virtual void FixedUpdate()
        // {
        //     // 在这里 Move() 比较卡，想解决的话需要插值
        // }

        protected virtual void LateUpdate()
        {
            FollowCharacterPosition();

            historyPosition[1] = historyPosition[0];
            historyPosition[0] = CharacterPosition;
            lastFrameVelocity = (historyPosition[0] - historyPosition[1]) / Time.deltaTime;
        }

        public event Action<GameplayEvent> GameplayEvent;

        public void OnGameplayEvent(GameplayEvent obj)
        {
            GameplayEvent?.Invoke(obj);
        }

        public virtual void Initialize(Character character)
        {
            Character = character;

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
            transform.position = CharacterPosition + data.controllerFollowOffset;
        }

        private void Move()
        {
            Character.Movement.DetectGround(lastFrameVelocity.Ground().normalized);
            // DebugE.LogValue(nameof(Character.Movement.GroundDistance), Character.Movement.GroundDistance);

            var motion = Vector3.zero;

            if (!Character.Movement.CurrentState.ShouldGrounded)
            {
                var v0 = Velocity;

                // Acceleration isn't constant, just an approximation.
                var acceleration = (useGravity ? Data.gravitationalAcceleration : Vector3.zero) +
                                   Data.airDrag.Mult(Velocity.Pow(2)).Mult(-Velocity.Sign());

                Velocity += acceleration * Time.deltaTime;
                motion = (v0 + Velocity) * Time.deltaTime / 2;
            }
            else
            {
                Velocity = lastFrameVelocity; // 下落时使用
                motion = Vector3.down * Character.UnityCharacterController.stepOffset;
            }

            // DebugE.LogValue(nameof(motion), motion);

            if (motion != Vector3.zero)
            {
                Character.UnityCharacterController.Move(motion);
            }
        }

        public void Jump()
        {
            Velocity = GroundVelocity + // 避免在斜坡上起跳时的影响
                       (Character.Movement.LastState is not StandState
                           ? Character.transform.forward * Data.jumpForwardSpeed
                           : Vector3.zero) + // 跳跃向前的速度
                       Vector3.up * Data.jumpUpSpeed; // 跳跃向上的速度
        }

        private void UpdateRotation()
        {
            if (LocalMoveDirection != Vector3.zero && CharacterTargetRotation != rotationTask.Target)
            {
                rotationTask.SetTask(CharacterRotation, CharacterTargetRotation, data.timeToRotate);
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