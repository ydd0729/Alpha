using System;
using UnityEngine;
using Yd.Animation;
using Yd.Audio;

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

            // context.Character.Controller.NavMeshAgent.enabled = false;
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

        public override void OnExit(ref MovementStateContext context)
        {
            base.OnExit(ref context);

            context.Character.SetGrounded(true);

            context.Character.AudioManager.PlayOneShot(AudioId.Land, AudioChannel.World);

            // context.Character.Controller.NavMeshAgent.enabled = true;
        }
    }
}