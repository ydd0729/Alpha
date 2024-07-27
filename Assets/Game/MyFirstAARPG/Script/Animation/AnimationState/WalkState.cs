using System;
using Shared.Collections;
using Shared.Pattern;

namespace MyFirstAARPG
{
    public class WalkState : AnimationState
    {
        public WalkState() : base(stateParameterName: AnimatorParameterName.Walk, rootMotionOnEnter: true)
        {
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
                FallState or StandState or RunState or JumpState => true,
                WalkState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}