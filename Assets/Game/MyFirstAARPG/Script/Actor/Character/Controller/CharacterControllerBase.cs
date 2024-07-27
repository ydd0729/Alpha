using System;
using Shared.Extension;
using UnityEngine;

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
        [SerializeField] private Vector3 AirDrag = new(0.01f, 0.003f, 0.01f);
        [SerializeField] private float maxFallSpeed = 15;
        
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

                    FallVelocity = IsInAir ? Character.CharacterController.velocity : Vector3.zero;
                }
            }
        }
        
        private bool isInAir;
        private Vector3 FallVelocity;
        private static readonly Vector3[] GroundDetectionSidesOffset = { new(0, 1.1f, 0), new(1.1f, 1.1f, 0), new(-1.1f, 1.1f, 0), new(0, 1.1f, 1.1f), new(0, 1.1f, -1.1f) };
        
        // Animation
        
        [SerializeField] private float jumpHeight = 1; 
        [SerializeField] private float jumpTime = 0.1f;
        
        protected Character Character { get; private set; }
        protected Vector3 LocalMoveDirection { get; set; } = Vector3.zero;
        protected EWalkRunToggle WalkRunToggle;
        
        private Vector3 CharacterVisualPosition => Character.CharacterVisualObject.transform.position;
        private Quaternion CharacterTargetRotation => Quaternion.LookRotation(transform.TransformDirection(LocalMoveDirection).Ground());
        private AnimationState AnimationState
        {
            set
            {
                if (animationState != value)
                {
                    animationState = value;
                    Act?.Invoke(animationState);
                }
            }
        }
        private Quaternion VisualObjectRotation
        {
            get => Character.CharacterVisualObject.transform.rotation;
            set => Character.CharacterVisualObject.transform.rotation = value;
        }
        private float StepOffset => Character.CharacterController.stepOffset;
        private AnimationState animationState;

        // Events

        public event Action<bool> InAir;
        public event Action<AnimationState> Act;

        // Event Functions

        protected virtual void Update()
        {
            if (IsInAir)
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
            // DebugExtension.LogValue(nameof(FallVelocity), FallVelocity);
        }

        protected virtual void LateUpdate()
        {
            DetectGround();
            
            if (applyGravity)
            {
                ApplyGravity(); // animation should update before this
            }
            
            FollowCharacterVisualPosition();
        }

        // Methods

        public virtual void Initialize(Character inCharacter)
        {
            IsInAir = false;
            GroundDistance = 0;
            Character = inCharacter;
            FallVelocity = Vector3.zero;
            WalkRunToggle = EWalkRunToggle.Run;
            AnimationState = AnimationState.None;
            
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
            if (GroundDistance > StepOffset || FallVelocity != Vector3.zero)
            {
                var v0 = FallVelocity;
                // Acceleration isn't constant during this delta time, but this is an enough approximation of air resistance.
                var acceleration = gravitationalAcceleration - VectorExtension.Multiply(AirDrag, VectorExtension.Pow(v0, 2));
                FallVelocity += acceleration * Time.deltaTime;
                
                motion = (v0 + FallVelocity) * Time.fixedDeltaTime / 2;
            }
            else
            {
                motion = Vector3.down * GroundDistance;
            }

            Character.CharacterController.Move(motion);
            
            // DebugExtension.LogValue(nameof(Acceleration), Acceleration);
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