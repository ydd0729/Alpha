using System;
using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class FallState : MovementState
    {
        public FallState() : base(AnimatorParameterId.Fall, shouldOnGround: false)
        {
        }

        public override void Tick(ref MovementStateTransitionContext context)
        {
            base.Tick(ref context);

            if (context.IsGrounded)
            {
                if (context.CharacterController.Velocity != Vector3.zero)
                {
                    context.CharacterMovement.TryTransitTo
                    (
                        context.CharacterController.WalkRunToggle switch
                        {
                            EWalkRunToggle.Walk => Walk,
                            EWalkRunToggle.Run => Run,
                            _ => throw new ArgumentOutOfRangeException()
                        }
                    );
                }
                else
                {
                    context.CharacterMovement.TryTransitTo(Stand);
                }
            }
        }

        public override bool CanTransit(MovementStateTransitionContext context)
        {
            if (!base.CanTransit(context))
            {
                return false;
            }

            //if (context.IsGrounded)
            //{
            //    return false;
            //}

            return context.CurrentState switch
            {
                JumpState or StandState or WalkState or RunState => true,
                FallState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}