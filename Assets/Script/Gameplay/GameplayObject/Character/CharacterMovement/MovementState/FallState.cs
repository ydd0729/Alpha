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

        public override void OnEnter(ref MovementStateContext context)
        {
            base.OnEnter(ref context);

            if (context.LastOrNextState is not JumpState)
            {
                context.Character.SetGrounded(false);
            }
        }

        public override void OnTick(ref MovementStateContext context)
        {
            base.OnTick(ref context);

            if (context.Character.IsGrounded)
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

        // public override bool CanTransitFrom(MovementStateTransitionContext context)
        // {
        //     if (!base.CanTransitFrom(context))
        //     {
        //         return false;
        //     }
        //
        //     //if (context.IsGrounded)
        //     //{
        //     //    return false;
        //     //}
        //
        //     return context.CurrentState switch
        //     {
        //         JumpState or StandState or WalkState or RunState => true,
        //         FallState => false,
        //         _ => throw new NotImplementedException()
        //     };
        // }

        public override void OnExit(ref MovementStateContext context)
        {
            base.OnExit(ref context);

            context.Character.SetGrounded(true);
        }
    }
}