using System;
using Shared.Extension;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyFirstAARPG
{
    public class CharacterControllerBase : MonoBehaviour
    {
        // Rotation

        [SerializeField] private float timeToRotate = 0.1f;

        private RotationTask rotationTask;

        // Fall

        [SerializeField] private bool applyGravity = true;
        [SerializeField] private Vector3 gravitationalAcceleration = new(0, -9.8f, 0);
        [SerializeField] private Vector3 airDrag = new(0.01f, 0.003f, 0.01f);
        [SerializeField] protected Vector3 initialJumpVelocity = Vector3.up * 4;

        public float GroundDistance { get; private set; }

        public bool IsInAir
        {
            get => isInAir;
            private set
            {
                if (isInAir != value)
                {
                    isInAir = value;
                    InAir?.Invoke(isInAir);

                    if (!IsInAir)
                    {
                        inAirVelocity = Vector3.zero;

                        if (Jumping)
                        {
                            Jumping = false;
                        }
                    }
                    else if (!Jumping)
                    {
                        inAirVelocity = LastFrameVelocity;
                    }
                }
            }
        }

        private bool isInAir;
        private Vector3 inAirVelocity;
        private static readonly Vector3[] GroundDetectionSidesOffset = { new(0, 1.1f, 0), new(1.1f, 1.1f, 0), new(-1.1f, 1.1f, 0), new(0, 1.1f, 1.1f), new(0, 1.1f, -1.1f) };

        // Animation
        
        protected Character Character { get; private set; }
        protected Vector3 LocalMoveDirection { get; set; } = Vector3.zero;
        protected EWalkRunToggle WalkRunToggle;

        protected AnimationState AnimationState
        {
            set
            {
                if (animationState != value)
                {
                    animationState = value;

                    switch (animationState) 
                    {
                        case JumpState:
                            inAirVelocity = LastFrameVelocity + initialJumpVelocity;
                            Jumping = true;
                            break;
                    }

                    Act?.Invoke(animationState);
                }
            }
        }

        protected Vector3 LastFrameVelocity => (historyPosition[0] - historyPosition[1]) / lastFrameDeltaTime;

        private Vector3 CharacterVisualPosition => Character.CharacterVisualObject.transform.position;
        private Quaternion CharacterTargetRotation => Quaternion.LookRotation(transform.TransformDirection(LocalMoveDirection).Ground());

        private Quaternion VisualObjectRotation
        {
            get => Character.CharacterVisualObject.transform.rotation;
            set => Character.CharacterVisualObject.transform.rotation = value;
        }

        private float StepOffset => Character.CharacterController.stepOffset;
        private AnimationState animationState;
        private bool Jumping;
        private Vector3[] historyPosition = new Vector3[2];
        private float lastFrameDeltaTime;

        // Events

        public event Action<bool> InAir;
        public event Action<AnimationState> Act;

        // Event Functions

        protected virtual void Update()
        {
            if (IsInAir)
            {
            }
            else if (Jumping)
            {
                
            }
            else if (LocalMoveDirection != Vector3.zero)
            {
                if (CharacterTargetRotation != rotationTask.Target)
                {
                    rotationTask.SetTask(VisualObjectRotation, CharacterTargetRotation, timeToRotate);
                }

                switch (WalkRunToggle)
                {
                    case EWalkRunToggle.Walk:
                        AnimationState = AnimationState.Walk;
                        break;
                    case EWalkRunToggle.Run:
                        AnimationState = AnimationState.Run;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                AnimationState = AnimationState.Stand;
            }

            if (rotationTask.Executing)
            {
                VisualObjectRotation = rotationTask.Execute();
            }

            // DebugExtension.LogValue(nameof(LocalMoveDirection), LocalMoveDirection);
            // DebugExtension.LogValue(nameof(GroundDistance), GroundDistance);
            // DebugExtension.LogValue(nameof(IsInAir), IsInAir);
            // DebugExtension.LogValue(nameof(Character.CharacterController.isGrounded), Character.CharacterController.isGrounded);
            DebugExtension.LogValue(nameof(inAirVelocity), inAirVelocity);
            DebugExtension.LogValue(nameof(LastFrameVelocity), LastFrameVelocity);
        }

        protected virtual void LateUpdate()
        {
            DetectGround();

            if (applyGravity)
            {
                ApplyGravity(); // animation should update before this
            }

            historyPosition[1] = historyPosition[0];
            historyPosition[0] = CharacterVisualPosition;
            lastFrameDeltaTime = Time.deltaTime;
            
            FollowCharacterVisualPosition();
        }

        // Methods

        public virtual void Initialize(Character inCharacter)
        {
            IsInAir = false;
            GroundDistance = 0;
            Character = inCharacter;
            inAirVelocity = Vector3.zero;
            WalkRunToggle = EWalkRunToggle.Run;
            AnimationState = AnimationState.None;
            Jumping = false;
            lastFrameDeltaTime = Mathf.Infinity;
            for (int i = 0; i < 2; i++)
            {
                historyPosition[i] = CharacterVisualPosition;
            }

            Character.CharacterAnimation.Fall += OnFall;

            FollowCharacterVisualPosition();
        }

        private void OnFall()
        {
            AnimationState = AnimationState.Fall;
        }

        private void FollowCharacterVisualPosition()
        {
            transform.position = CharacterVisualPosition + Character.CharacterVisual.ControllerFollowOffset;
        }

        private void DetectGround()
        {
            var layerMask = 0;
            layerMask = ~layerMask;

            var tempGroundDistance = Mathf.Infinity;
            foreach (var sideOffset in GroundDetectionSidesOffset)
            {
                var origin = CharacterVisualPosition + Character.CharacterController.radius * sideOffset;
                PhysicsExtension.Raycast(origin, Vector3.down, out var hitInfo, Mathf.Infinity, layerMask, true, Color.yellow, Color.white);

                tempGroundDistance = Mathf.Min(tempGroundDistance, hitInfo.distance - Character.CharacterController.radius * sideOffset.y);
            }

            GroundDistance = tempGroundDistance;

            IsInAir = GroundDistance > StepOffset;

            // DebugExtension.LogValue(nameof(IsInAir), IsInAir);
        }

        private void ApplyGravity()
        {
            Vector3 motion;
            if (GroundDistance > StepOffset || inAirVelocity != Vector3.zero)
            {
                var v0 = inAirVelocity;
                var fallDirection = inAirVelocity.normalized;
                // Acceleration isn't constant during this delta time, but this is an enough approximation of air resistance.
                var acceleration = gravitationalAcceleration - VectorExtension.Multiply(VectorExtension.Multiply(airDrag, VectorExtension.Pow(v0, 2)), MathfExtension.Sign(v0));
                inAirVelocity += acceleration * Time.deltaTime;

                motion = (v0 + inAirVelocity) * Time.fixedDeltaTime / 2;
            }
            else
            {
                motion = Vector3.down * GroundDistance;
            }

            Character.CharacterController.Move(motion);

            // DebugExtension.LogValue(nameof(inAirVelocity), inAirVelocity);
        }

        private struct RotationTask
        {
            private Quaternion origin;
            private float rotationTime;
            private float timePassed;

            public Quaternion Target { get; private set; }
            public bool Executing { get; private set; }

            public void SetTask(Quaternion inOrigin, Quaternion inTarget, float timeToRotateHalfCircle)
            {
                origin = inOrigin;
                Target = inTarget;
                rotationTime = timeToRotateHalfCircle;
                timePassed = 0;
                Executing = true;
            }

            public Quaternion Execute()
            {
                timePassed += Time.deltaTime;
                if (timePassed >= rotationTime)
                {
                    Executing = false;
                }

                return Quaternion.Slerp(origin, Target, Math.Min(timePassed / rotationTime, 1));
            }
        }

        protected enum EWalkRunToggle
        {
            Walk,
            Run
        }
    }
}