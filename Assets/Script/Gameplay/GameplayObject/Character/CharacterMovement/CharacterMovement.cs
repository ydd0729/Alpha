using System;
using Script.Gameplay.Sound;
using UnityEngine;
using Yd.Animation;
using Yd.Audio;
using Yd.Extension;
using Yd.PhysicsExtension;

namespace Yd.Gameplay.Object
{
    public class CharacterMovement : MonoBehaviour
    {
        private AnimationEventListener animationEventListener;
        public MovementStateContext context;

        // public bool IsGrounded => context.IsGrounded;
        public Character Character => context.Character;
        public MovementState CurrentState => context.CurrentState;
        public MovementState LastOrNextState => context.LastOrNextState;
        public float GroundDistance => context.GroundDistance;
        public bool IsWalkingOrRunning => CurrentState is WalkState or RunState;

        // private void Start()
        // {
        //     // DetectGround();
        // }

        private void Update()
        {
            DetectGround();
            Character.Animator.SetValue(AnimatorParameterId.GroundVelocity, Character.Controller.GroundVelocity.magnitude);

            CurrentState.OnTick(ref context);
        }

        public event Action<MovementStateContext> MovementStateChanged;

        public void Initialize(Character character)
        {
            context.Character = character;
            context.CurrentState = MovementState.Stand;
            context.GroundDistance = 0;

            animationEventListener = gameObject.GetOrAddComponent<AnimationEventListener>();
            animationEventListener.GameplayEventDispatcher += OnStep;

            character.AnimatorMoved += OnAnimatorMoved;

            // var characterAnimationRandomizer = gameObject.AddComponent<CharacterAnimationRandomizer>();
            // characterAnimationRandomizer.Initialize(Character);
        }

        public void RequestJump()
        {
            if (Character.Controller.AllowMovement && CurrentState is not (JumpState or FallState))
            {
                context.JumpRequested = true;
            }
        }

        private void OnAnimatorMoved()
        {
            if (Character.Animator.applyRootMotion)
            {
                Character.Animator.ApplyBuiltinRootMotion();
            }
        }

        private void TransitTo(MovementState nextState)
        {
            if (nextState == CurrentState)
            {
                return;
            }

            context.LastOrNextState = nextState;
            CurrentState?.OnExit(ref context);

            context.LastOrNextState = context.CurrentState;
            context.CurrentState = nextState;
            CurrentState?.OnEnter(ref context);

            MovementStateChanged?.Invoke(context);
        }

        public bool TryTransitTo(MovementState state)
        {
            // if (state.CanTransitFrom(context))
            // {
            TransitTo(state);
            //     return true;
            // }

            return false;
        }

        public void DetectGround()
        {
            var radius = Character.UnityController.radius;
            var origin = Character.transform.position + Vector3.up * (radius + 0.01f);
            var layerMask = Physics.DefaultRaycastLayers & ~LayerMaskE.Character;

            if (PhysicsE.SphereCast
                (
                    origin,
                    radius,
                    Vector3.down,
                    out var hitInfo,
                    layerMask: layerMask,
                    drawDebug: true,
                    hitColor: Color.gray,
                    segment: 8
                ))
            {
                Character.FootstepAudioId = hitInfo.collider.gameObject.tag switch
                {
                    "Grass" => AudioId.GrassFootstep,
                    "Stone" => AudioId.StoneFootstep,
                    "Sand" => AudioId.SandFootstep,
                    _ => Character.FootstepAudioId
                };
                // Debug.Log(FootstepAudioId);
                
                context.GroundDistance = Character.transform.position.y - hitInfo.point.y;
            }
            else
            {
                context.GroundDistance = Mathf.Infinity;
            }

            // Debug.Log(GroundDistance);
        }

        private void OnStep(GameplayEventType eventType)
        {
            var humanoidCharacter = Character as HumanoidCharacter;

            switch(eventType)
            {
                case GameplayEventType.StepLeft:
                    Character.Animator.SetValue(AnimatorParameterId.StepLeft, true);
                    Character.Animator.SetValue(AnimatorParameterId.StepRight, false);

                    if (humanoidCharacter != null)
                    {
                        humanoidCharacter.PlayGameplaySound(GameplaySound.Step);
                    }
                    break;
                case GameplayEventType.StepRight:
                    Character.Animator.SetValue(AnimatorParameterId.StepLeft, false);
                    Character.Animator.SetValue(AnimatorParameterId.StepRight, true);

                    if (humanoidCharacter != null)
                    {
                        humanoidCharacter.PlayGameplaySound(GameplaySound.Step);
                    }
                    break;
            }
        }
    }
}