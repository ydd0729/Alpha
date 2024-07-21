using System;
using System.Collections.Generic;
using Shared.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyFirstAARPG
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] private AnimatorData data;

        [SerializeField] private AnimationClipEvent animationClipEvent;
        
        private void Start()
        {
            animationClipEvent.Step += AnimationClipEventOnStep;
        }

        public event EventHandler<AnimationStateChangedEventArgs> AnimationStateChanged;

        public Animator Animator
        {
            get => animator;
            set => animator = value;
        }

        public AnimationClipEvent AnimationClipEvent
        {
            get => animationClipEvent;
            set => animationClipEvent = value;
        }

        public AnimatorData Data => data;

        public AnimationState CurrentState { get; set; }

        public void Walk()
        {
            TransitionTo(WalkState);
        }
        
        public void Run()
        {
            TransitionTo(RunState);
        }

        public void Stand()
        {
            TransitionTo(StandState);
        }

        public void TransitionTo(AnimationState nextState)
        {
            if (nextState == CurrentState)
            {
                return;
            }
            
            // AnimatorParameter OldStateParameter = CurrentState.StateParameter;
            AnimatorParameterName currentStateParameterName = nextState.StateParameterName;

            CurrentState?.Exit();
            CurrentState = nextState;
            CurrentState.Enter();
            

            OnAnimationStateChanged(new()
            {
                // OldStateParameter = OldStateParameter, 
                CurrentStateParameterName = currentStateParameterName
            });
        }
        
        private WalkState walkState;
        private RunState runState;
        private StandState standState;

        private WalkState WalkState => walkState ??= new(this);
        private RunState RunState => runState ??= new(this);
        private StandState StandState => standState ??= new(this);
        
        private void OnAnimationStateChanged(AnimationStateChangedEventArgs e)
        {
            AnimationStateChanged?.Invoke(this, e);
        }
        
        private void AnimationClipEventOnStep(object _, StepEventArgs args)
        {
            switch (args.AnimationClipEventType)
            {
                case AnimationClipEventType.StepLeft:
                {
                    animator.SetBool(AnimatorParameterName.StepLeft.GetId(), true); 
                    animator.SetBool(AnimatorParameterName.StepRight.GetId(), false); 
                    break;
                }
                case AnimationClipEventType.StepRight:
                {
                    animator.SetBool(AnimatorParameterName.StepLeft.GetId(), false); 
                    animator.SetBool(AnimatorParameterName.StepRight.GetId(), true);
                    break;
                }
                case AnimationClipEventType.StepLeftMiddle:
                {
                    animator.SetBool(AnimatorParameterName.StepLeftMiddle.GetId(), true); 
                    animator.SetBool(AnimatorParameterName.StepRightMiddle.GetId(), false);
                    break;
                }
                case AnimationClipEventType.StepRightMiddle:
                {
                    animator.SetBool(AnimatorParameterName.StepLeftMiddle.GetId(), false); 
                    animator.SetBool(AnimatorParameterName.StepRightMiddle.GetId(), true);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public class AnimationStateChangedEventArgs : EventArgs
    {
        // public AnimatorParameter OldStateParameter;
        public AnimatorParameterName CurrentStateParameterName;
    }
}