using System;
using Shared.Collections;
using Shared.Pattern;

namespace MyFirstAARPG
{
    public class JumpState : AnimationState
    {
        public JumpState() : base(stateParameterName: AnimatorParameterName.Jump, rootMotionOnEnter: false)
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
                StandState or WalkState or RunState => true,
                JumpState or FallState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}