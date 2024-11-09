using UnityEngine;
using Yd.Animation;

namespace Yd.Gameplay.Object
{
    public class MovementState
    {
        public static readonly MovementState None = new();
        public static readonly WalkState Walk = new();
        public static readonly RunState Run = new();
        public static readonly StandState Stand = new();
        public static readonly JumpState Jump = new();
        public static readonly FallState Fall = new();

        public readonly AnimatorParameterId StateParameterName;

        protected MovementState(
            AnimatorParameterId stateParameterName = AnimatorParameterId.None, bool allowRotation = false,
            bool shouldOnGround = true
        )
        {
            StateParameterName = stateParameterName;
            AllowRotation = allowRotation;
            IsGroundState = shouldOnGround;
        }


        public bool AllowRotation { get; private set; }

        public bool IsGroundState { get; private set; }

        public virtual bool CanTransitFrom(MovementStateTransitionContext context)
        {
            return true;
        }

        public virtual void OnEnter(ref MovementStateTransitionContext context)
        {
            context.EnterTime = Time.time;

            var animator = context.Character.Animator;

            animator.SetValue(StateParameterName, true);
            animator.SetValue(AnimatorParameterId.RandomIndex, 0);
        }

        public virtual void OnTick(ref MovementStateTransitionContext context)
        {
        }

        public virtual void OnExit(ref MovementStateTransitionContext context)
        {
            var animator = context.Character.Animator;

            animator.SetValue(StateParameterName, false);

        }
    }
}