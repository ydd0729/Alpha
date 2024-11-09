using System;
using UnityEngine;
using Yd.Animation;
using Yd.Audio;
using Yd.Extension;
using AnimationEvent = Yd.Animation.AnimationEvent;

namespace Yd.Gameplay.Object
{
    public class CharacterMovement : MonoBehaviour
    {
        private AnimationEventDispatcher animationEventDispatcher;
        private MovementStateTransitionContext context;

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
            // DetectGround();
            Character.Animator.SetValue(AnimatorParameterId.GroundVelocity, Character.Controller.GroundVelocity.magnitude);

            CurrentState.OnTick(ref context);
        }

        public event Action<MovementStateTransitionContext> MovementStateChanged;

        public void Initialize(Character character)
        {
            context.Character = character;
            context.CurrentState = MovementState.Stand;
            context.GroundDistance = 0;

            animationEventDispatcher = gameObject.GetOrAddComponent<AnimationEventDispatcher>();
            animationEventDispatcher.Step += OnStep;

            character.AnimatorMoved += OnAnimatorMoved;

            // var characterAnimationRandomizer = gameObject.AddComponent<CharacterAnimationRandomizer>();
            // characterAnimationRandomizer.Initialize(Character);
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
            if (state.CanTransitFrom(context))
            {
                TransitTo(state);
                return true;
            }

            return false;
        }

        public void DetectGround()
        {
            var radius = Character.UnityController.radius;
            var origin = Character.transform.position + Vector3.up * radius;

            if (PhysicsE.SphereCast(origin, 0.1f, Vector3.down, out var hitInfo, true, Color.red, Color.blue, 32))
            {
                context.GroundDistance = Character.transform.position.y - hitInfo.point.y;
            }
            else
            {
                context.GroundDistance = Mathf.Infinity;
            }

            // DebugE.LogValue(nameof(GroundDistance), GroundDistance);
        }

        private void OnStep(AnimationEvent @event)
        {
            var humanoidCharacter = Character as HumanoidCharacter;

            switch(@event)
            {
                case AnimationEvent.StepLeft:
                    Character.Animator.SetValue(AnimatorParameterId.StepLeft, true);
                    Character.Animator.SetValue(AnimatorParameterId.StepRight, false);

                    if (humanoidCharacter != null)
                    {
                        humanoidCharacter.AudioManager.PlayOneShot
                            (AudioId.StoneFootstep, AudioChannel.FootStep, humanoidCharacter.LeftFoot);
                    }
                    break;
                case AnimationEvent.StepRight:
                    Character.Animator.SetValue(AnimatorParameterId.StepLeft, false);
                    Character.Animator.SetValue(AnimatorParameterId.StepRight, true);

                    if (humanoidCharacter != null)
                    {
                        humanoidCharacter.AudioManager.PlayOneShot
                            (AudioId.StoneFootstep, AudioChannel.FootStep, humanoidCharacter.RightFoot);
                    }
                    break;
                // case AnimationEvent.StepLeftMiddle:
                //     Character.Animator.SetValue(AnimatorParameterId.StepLeftMiddle, true);
                //     Character.Animator.SetValue(AnimatorParameterId.StepRightMiddle, false);
                //     break;
                // case AnimationEvent.StepRightMiddle:
                //     Character.Animator.SetValue(AnimatorParameterId.StepLeftMiddle, false);
                //     Character.Animator.SetValue(AnimatorParameterId.StepRightMiddle, true);
                //     break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}