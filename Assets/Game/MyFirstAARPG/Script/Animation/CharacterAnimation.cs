using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstAARPG
{
    public class CharacterAnimation : MonoBehaviour
    {
        // Fields

        [SerializeField] private Animator animator;
        [SerializeField] private AnimatorData data;
        [SerializeField] private AnimationClipEvent animationClipEvent;
        
        private AnimationState currentState;
        private AnimationStateTransitionContext animationStateTransitionContext;

        // Properties

        public Animator Animator
        {
            get => animator;
            set
            {
                animator = value;
                animationStateTransitionContext.Animator = animator;
            }
        }

        public AnimatorData Data => data;

        public AnimationState CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;
                animationStateTransitionContext.CurrentState = currentState;
            }
        }
        
        // Events

        public event Action<AnimationStateChangedEventArgs> AnimationStateChanged;
        public event Action Fall;

        // Methods

        public void Initialize(Animator inAnimator, AnimationClipEvent inAnimationClipEvent, CharacterControllerBase characterControllerBase)
        {
            Animator = inAnimator;
            CurrentState = AnimationState.Stand;

            animationClipEvent = inAnimationClipEvent;
            animationClipEvent.Step += OnStep;

            characterControllerBase.InAir += OnInAir;
            characterControllerBase.Act += OnCharacterAct;
        }

        private void TransitTo(AnimationState nextState)
        {
            if (nextState == CurrentState)
            {
                return;
            }

            var OldStateParameter = CurrentState.StateParameterName;
            var currentStateParameterName = nextState.StateParameterName;

            CurrentState?.Exit(ref animationStateTransitionContext);
            CurrentState = nextState;
            CurrentState.Enter(ref animationStateTransitionContext);


            AnimationStateChanged?.Invoke(new AnimationStateChangedEventArgs
            {
                OldStateParameterName = OldStateParameter,
                CurrentStateParameterName = currentStateParameterName
            });
        }

        private void OnCharacterAct(AnimationState animationState)
        {
            if (animationState.CanTransit(animationStateTransitionContext))
            {
                TransitTo(animationState);
            }
        }

        private void OnInAir(bool isInAir)
        {
            animationStateTransitionContext.IsInAir = isInAir;

            if (animationStateTransitionContext.IsInAir)
            {
                if (AnimationState.Fall.CanTransit(animationStateTransitionContext))
                {
                    Fall?.Invoke();
                    TransitTo(AnimationState.Fall);
                }
            }
        }

        private void OnStep(object _, StepEventArgs args)
        {
            switch (args.AnimationClipEventType)
            {
                case AnimationClipEventType.StepLeft:
                {
                    Animator.SetBool(AnimatorParameterName.StepLeft.GetId(), true);
                    Animator.SetBool(AnimatorParameterName.StepRight.GetId(), false);
                    break;
                }
                case AnimationClipEventType.StepRight:
                {
                    Animator.SetBool(AnimatorParameterName.StepLeft.GetId(), false);
                    Animator.SetBool(AnimatorParameterName.StepRight.GetId(), true);
                    break;
                }
                case AnimationClipEventType.StepLeftMiddle:
                {
                    Animator.SetBool(AnimatorParameterName.StepLeftMiddle.GetId(), true);
                    Animator.SetBool(AnimatorParameterName.StepRightMiddle.GetId(), false);
                    break;
                }
                case AnimationClipEventType.StepRightMiddle:
                {
                    Animator.SetBool(AnimatorParameterName.StepLeftMiddle.GetId(), false);
                    Animator.SetBool(AnimatorParameterName.StepRightMiddle.GetId(), true);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class AnimationStateChangedEventArgs : EventArgs
    {
        public AnimatorParameterName OldStateParameterName;
        public AnimatorParameterName CurrentStateParameterName;
    }
}