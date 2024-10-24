using System;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class JumpState : MovementState
    {
        public JumpState() : base(AnimatorParameterId.Jump, shouldOnGround: false)
        {
        }

        public override void Tick(ref MovementStateTransitionContext context)
        {
            base.Tick(ref context);

            if (context.Character.Controller.Velocity.y <= 0)
            {
                context.CharacterMovement.TryTransitTo(Fall);
            }
        }

        public override bool CanTransit(MovementStateTransitionContext context)
        {
            if (!base.CanTransit(context))
            {
                return false;
            }

            if (!context.IsGrounded)
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