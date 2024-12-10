using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class WalkState : MovementState
    {
        public WalkState() : base(AnimatorParameterId.Walk, true)
        {
        }

        public override void OnTick(ref MovementStateContext context)
        {
            base.OnTick(ref context);

            if (!context.Character.IsGrounded)
            {
                context.Character.Movement.TryTransitTo(Fall);
            }
            else if (context.JumpRequested)
            {
                context.JumpRequested = false;
                context.Character.Movement.TryTransitTo(Jump);
            }
            else if (context.CharacterController.LocalMoveDirection == Vector3.zero)
            {
                context.Character.Movement.TryTransitTo(Stand);
            }
            else if (context.CharacterController.WalkRunToggle is EWalkRunToggle.Run)
            {
                context.Character.Movement.TryTransitTo(Run);
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
        //         FallState or StandState or RunState or JumpState => true,
        //         WalkState => false,
        //         _ => throw new NotImplementedException()
        //     };
        // }
    }
}