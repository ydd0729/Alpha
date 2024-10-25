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
            ShouldGrounded = shouldOnGround;
        }


        public bool AllowRotation { get; private set; }

        public bool ShouldGrounded { get; private set; }

        public virtual bool CanTransit(MovementStateTransitionContext context)
        {
            return true;
        }

        public virtual void Enter(ref MovementStateTransitionContext context)
        {
            var animator = context.Character.Animator;

            animator.SetBool(StateParameterName.GetAnimatorHash(), true);

            animator.SetValue(AnimatorParameterId.RandomIndex, 0);
            animator.SetValue(AnimatorParameterId.LastIndex, 0);
        }

        public virtual void Tick(ref MovementStateTransitionContext context)
        {
        }

        public virtual void Exit(ref MovementStateTransitionContext context)
        {
            var animator = context.Character.Animator;

            animator.SetValue(StateParameterName, false);
            
        }
    }
}