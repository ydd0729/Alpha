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
        private MovementStateTransitionContext transitionContext;

        public bool IsGrounded => transitionContext.IsGrounded;
        public Character Character => transitionContext.Character;
        public MovementState CurrentState => transitionContext.CurrentState;
        public MovementState LastState => transitionContext.LastState;
        public float GroundDistance => transitionContext.GroundDistance;
        public bool IsWalkingOrRunning => CurrentState is WalkState or RunState;

        // private void Start()
        // {
        //     // DetectGround();
        // }

        private void Update()
        {
            // DetectGround();
            Character.Animator.SetValue(AnimatorParameterId.GroundVelocity, Character.Controller.GroundVelocity.magnitude);

            CurrentState.Tick(ref transitionContext);
        }

        public event Action<MovementStateChangedEventArgs> MovementStateChanged;

        public void Initialize(Character character)
        {
            transitionContext.Character = character;
            transitionContext.CurrentState = MovementState.Stand;
            transitionContext.GroundDistance = 0;

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

            var oldStateParameter = CurrentState.StateParameterName;
            var currentStateParameterName = nextState.StateParameterName;

            CurrentState?.Exit(ref transitionContext);
            transitionContext.LastState = transitionContext.CurrentState;
            transitionContext.CurrentState = nextState;
            CurrentState.Enter(ref transitionContext);


            MovementStateChanged?.Invoke
            (
                new MovementStateChangedEventArgs
                {
                    OldStateParameterName = oldStateParameter,
                    CurrentStateParameterName = currentStateParameterName
                }
            );
        }

        public bool TryTransitTo(MovementState state)
        {
            if (state.CanTransit(transitionContext))
            {
                TransitTo(state);
                return true;
            }

            return false;
        }

        public void DetectGround()
        {
            var radius = Character.UnityCharacterController.radius;
            var origin = Character.transform.position + Vector3.up * radius;

            if (PhysicsE.SphereCast(origin, 0.1f, Vector3.down, out var hitInfo, true, Color.red, Color.blue, 32))
            {
                transitionContext.GroundDistance = Character.transform.position.y - hitInfo.point.y;
            }
            else
            {
                transitionContext.GroundDistance = Mathf.Infinity;
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
                case AnimationEvent.StepLeftMiddle:
                    Character.Animator.SetValue(AnimatorParameterId.StepLeftMiddle, true);
                    Character.Animator.SetValue(AnimatorParameterId.StepRightMiddle, false);
                    break;
                case AnimationEvent.StepRightMiddle:
                    Character.Animator.SetValue(AnimatorParameterId.StepLeftMiddle, false);
                    Character.Animator.SetValue(AnimatorParameterId.StepRightMiddle, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class MovementStateChangedEventArgs : EventArgs
    {
        public AnimatorParameterId CurrentStateParameterName;
        public AnimatorParameterId OldStateParameterName;
    }
}