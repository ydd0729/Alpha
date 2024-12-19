using Yd.Animation;
using Yd.Audio;

namespace Yd.Gameplay.Object
{
    public class JumpState : MovementState
    {
        public JumpState() : base(AnimatorParameterId.Jump, shouldOnGround: false)
        {
        }

        public override void OnEnter(ref MovementStateContext context)
        {
            base.OnEnter(ref context);

            context.Character.SetGrounded(false);
            context.Character.AudioManager.PlayOneShot(AudioId.JumpUp, AudioChannel.World);
            // context.Character.Controller.NavMeshAgent.enabled = false;
        }

        public override void OnTick(ref MovementStateContext context)
        {
            base.OnTick(ref context);

            if (context.Timer > 0.1f && context.Character.Controller.Velocity.y <= 0)
            {
                context.CharacterMovement.TryTransitTo(Fall);
            }
        }

        public override void OnExit(ref MovementStateContext context)
        {
            base.OnExit(ref context);

            if (context.LastOrNextState is not FallState)
            {
                context.Character.SetGrounded(true);
            }
        }
    }
}