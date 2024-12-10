using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class JumpState : MovementState
    {
        public JumpState() : base(AnimatorParameterId.Jump, shouldOnGround: false)
        {
        }

        public override void OnEnter(ref MovementStateContext context)
        {
            base.OnEnter(ref context);

            context.Character.SetGrounded(false);
        }

        public override void OnTick(ref MovementStateContext context)
        {
            base.OnTick(ref context);

            if (context.Timer > 0.1f && context.Character.Controller.Velocity.y <= 0)
            {
                context.CharacterMovement.TryTransitTo(Fall);
            }
        }

        // public override bool CanTransitFrom(MovementStateTransitionContext context)
        // {
        //     if (!base.CanTransitFrom(context))
        //     {
        //         return false;
        //     }
        //
        //     if (!context.Character.IsGrounded)
        //     {
        //         return false;
        //     }
        //
        //     return context.CurrentState switch
        //     {
        //         StandState or WalkState or RunState => true,
        //         JumpState or FallState => false,
        //         _ => throw new NotImplementedException()
        //     };
        // }

        public override void OnExit(ref MovementStateContext context)
        {
            base.OnExit(ref context);

            if (context.LastOrNextState is not FallState)
            {
                context.Character.SetGrounded(true);
            }
        }
    }
}