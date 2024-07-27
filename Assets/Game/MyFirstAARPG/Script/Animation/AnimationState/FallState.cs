using System;

namespace MyFirstAARPG
{
    public class FallState : AnimationState
    {
        public FallState() : base(stateParameterName: AnimatorParameterName.Fall, rootMotionOnEnter: false)
        {
        }

        public override bool CanTransit(AnimationStateTransitionContext context)
        {
            if (!base.CanTransit(context))
            {
                return false;
            }

            if (!context.IsInAir)
            {
                return false;
            }

            return context.CurrentState switch
            {
                StandState or WalkState or RunState => true,
                JumpState or FallState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}