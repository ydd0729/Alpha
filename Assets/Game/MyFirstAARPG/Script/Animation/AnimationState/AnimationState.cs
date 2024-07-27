namespace MyFirstAARPG
{
    public class AnimationState
    {
        // Fields
        
        public static readonly AnimationState None = new();
        public static readonly WalkState Walk = new();
        public static readonly RunState Run = new();
        public static readonly StandState Stand = new();
        public static readonly JumpState Jump = new();
        public static readonly FallState Fall = new();
        
        public readonly AnimatorParameterName StateParameterName;
        
        protected readonly bool? RootMotionOnEnter;
        protected readonly bool? RootMotionOnExit;
        protected readonly bool RestoreRootMotionOnExit;
        
        private bool OldRootMotion;

        // Methods

        protected AnimationState(AnimatorParameterName stateParameterName = AnimatorParameterName.None, bool? rootMotionOnEnter = null, bool? rootMotionOnExit = null, bool restoreRootMotionOnExit = false)
        {
            RootMotionOnEnter = rootMotionOnEnter;
            RootMotionOnExit = rootMotionOnExit;
            RestoreRootMotionOnExit = restoreRootMotionOnExit;
            StateParameterName = stateParameterName;
        }

        public virtual bool CanTransit(AnimationStateTransitionContext context)
        {
            return true;
        }

        public virtual void Enter(ref AnimationStateTransitionContext context)
        {
            var animator = context.Animator;

            animator.SetBool(StateParameterName.GetId(), true);

            if (RootMotionOnEnter.HasValue)
            {
                if (RestoreRootMotionOnExit)
                {
                    OldRootMotion = animator.applyRootMotion;
                }

                animator.applyRootMotion = RootMotionOnEnter.Value;
            }

            animator.SetInteger(AnimatorParameterName.RandomIndex.GetId(), 0);
            animator.SetInteger(AnimatorParameterName.LastIndex.GetId(), 0);
        }

        public virtual void Exit(ref AnimationStateTransitionContext context)
        {
            var animator = context.Animator;

            animator.SetBool(StateParameterName.GetId(), false);

            if (RootMotionOnExit.HasValue)
            {
                animator.applyRootMotion = RootMotionOnExit.Value;
            }
            else if (RestoreRootMotionOnExit)
            {
                animator.applyRootMotion = OldRootMotion;
            }
        }
    }
}