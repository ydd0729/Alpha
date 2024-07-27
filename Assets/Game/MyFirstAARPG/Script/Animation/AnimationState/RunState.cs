using System;
using Shared.Collections;
using Shared.Pattern;

namespace MyFirstAARPG
{
    public class RunState : AnimationState
    {
        public RunState() : base(stateParameterName: AnimatorParameterName.Run, rootMotionOnEnter: true)
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
                FallState or StandState or WalkState or JumpState => true,
               RunState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}