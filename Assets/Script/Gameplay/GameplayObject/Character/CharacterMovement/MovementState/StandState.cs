using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class StandState : MovementState
    {
        public StandState() : base(AnimatorParameterId.Stand, true)
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
            else if (context.CharacterController.AllowMovement &&
                     context.CharacterController.LocalMoveDirection != Vector3.zero)
            {
                switch(context.CharacterController.WalkRunToggle)
                {
                    case EWalkRunToggle.Walk:
                        context.Character.Movement.TryTransitTo(Walk);
                        break;
                    case EWalkRunToggle.Run:
                        context.Character.Movement.TryTransitTo(Run);
                        break;
                }
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
        //         JumpState or FallState or WalkState or RunState => true,
        //         StandState => false,
        //         _ => throw new NotImplementedException()
        //     };
        // }
    }
}