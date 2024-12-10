using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class RunState : MovementState
    {
        public RunState() : base(AnimatorParameterId.Run, true)
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
            else if (context.CharacterController.WalkRunToggle is EWalkRunToggle.Walk)
            {
                context.Character.Movement.TryTransitTo(Walk);
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
        //         FallState or StandState or WalkState or JumpState => true,
        //         RunState => false,
        //         _ => throw new NotImplementedException()
        //     };
        // }
    }
}