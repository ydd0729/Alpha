using System;
using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class RunState : MovementState
    {
        public RunState() : base(AnimatorParameterId.Run, true)
        {
        }

        public override void Tick(ref MovementStateTransitionContext context)
        {
            base.Tick(ref context);

            if (!context.IsGrounded)
            {
                context.Character.Movement.TryTransitTo(Fall);
            }
            else if (context.Character.Controller.LocalMoveDirection == Vector3.zero)
            {
                context.Character.Movement.TryTransitTo(Stand);
            }
            else if (context.Character.Controller.WalkRunToggle is EWalkRunToggle.Walk)
            {
                context.Character.Movement.TryTransitTo(Walk);
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
                FallState or StandState or WalkState or JumpState => true,
                RunState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}