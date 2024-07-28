using System;
using Shared.Collections;
using Shared.Pattern;

namespace MyFirstAARPG
{
    public class StandState : AnimationState
    {
        public StandState() : base(stateParameterName: AnimatorParameterName.Stand, rootMotionOnEnter: true /* TODO */)
        {
        }

        public override void Enter(ref AnimationStateTransitionContext context)
        {
            base.Enter(ref context);
            
            context.Animator.SetBool(AnimatorParameterName.StepLeft.GetId(), false);
            context.Animator.SetBool(AnimatorParameterName.StepRight.GetId(), false);
        }

        public override bool CanTransit(AnimationStateTransitionContext context)
        {
            if (!base.CanTransit(context))
            {
                return false;
            }

            if (context.IsInAir)
            {
                return false;
            }

            return context.CurrentState switch
            {
                JumpState or FallState or WalkState or RunState => true,
                StandState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}