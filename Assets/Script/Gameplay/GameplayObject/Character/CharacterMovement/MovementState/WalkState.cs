using System;
using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class WalkState : MovementState
    {
        public WalkState() : base(AnimatorParameterId.Walk, true, true)
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
            else if (context.Character.Controller.WalkRunToggle is EWalkRunToggle.Run)
            {
                context.Character.Movement.TryTransitTo(Run);
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
                FallState or StandState or RunState or JumpState => true,
                WalkState => false,
                _ => throw new NotImplementedException()
            };
        }

        public override void Enter(ref MovementStateTransitionContext context)
        {
            base.Enter(ref context);
            
            context.CharacterController.OnGameplayEvent(GameplayEvent.Move);
        }
    }
}