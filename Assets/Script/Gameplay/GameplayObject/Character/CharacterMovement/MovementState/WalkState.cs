using System;
using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class WalkState : MovementState
    {
        public WalkState() : base(AnimatorParameterId.Walk, true)
        {
        }

        public override void OnTick(ref MovementStateTransitionContext context)
        {
            base.OnTick(ref context);

            if (!context.IsGrounded)
            {
                context.Character.Movement.TryTransitTo(Fall);
            }
            else if (context.Character.Controller.LocalMoveDirection == Vector3.zero)
            {
                context.Character.Movement.TryTransitTo(Stand);
            }
            else if (context.Character.Controller.WalkRunToggle is EWalkRunToggle.Run)
            {
                context.Character.Movement.TryTransitTo(Run);
            }
        }

        public override bool CanTransitFrom(MovementStateTransitionContext context)
        {
            if (!base.CanTransitFrom(context))
            {
                return false;
            }

            if (!context.IsGrounded)
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