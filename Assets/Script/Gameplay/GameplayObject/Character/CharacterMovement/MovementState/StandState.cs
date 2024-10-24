using System;
using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class StandState : MovementState
    {
        public StandState() : base(AnimatorParameterId.Stand, true, true)
        {
        }

        public override void Tick(ref MovementStateTransitionContext context)
        {
            base.Tick(ref context);

            if (!context.IsGrounded)
            {
                context.Character.Movement.TryTransitTo(Fall);
            }
            else if (context.CharacterController.AllowMovement && context.Character.Controller.LocalMoveDirection != Vector3.zero)
            {
                switch(context.Character.Controller.WalkRunToggle)
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
                JumpState or FallState or WalkState or RunState => true,
                StandState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}