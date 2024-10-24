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

        // protected readonly bool RestoreRootMotionOnExit;

        // protected readonly bool? RootMotionOnEnter;
        // protected readonly bool? RootMotionOnExit;

        public readonly AnimatorParameterId StateParameterName;

        protected MovementState(
            AnimatorParameterId stateParameterName = AnimatorParameterId.None, /*bool? rootMotionOnEnter = null,
            bool? rootMotionOnExit = null, bool restoreRootMotionOnExit = false,*/ bool allowRotation = false,
            bool shouldOnGround = true
        )
        {
            // RootMotionOnEnter = rootMotionOnEnter;
            // RootMotionOnExit = rootMotionOnExit;
            // RestoreRootMotionOnExit = restoreRootMotionOnExit;
            StateParameterName = stateParameterName;
            AllowRotation = allowRotation;
            ShouldGrounded = shouldOnGround;
        }


        public bool AllowRotation { get; private set; }

        public bool ShouldGrounded { get; private set; }

        // private bool oldRootMotion;

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        // private static void Init()
        // {
        //     None = new MovementState();
        //     Walk = new WalkState();
        //     Run = new RunState();
        //     Stand = new StandState();
        //     Jump = new JumpState();
        //     Fall = new FallState();
        // }

        public virtual bool CanTransit(MovementStateTransitionContext context)
        {
            return true;
        }

        public virtual void Enter(ref MovementStateTransitionContext context)
        {
            var animator = context.Character.Animator;

            animator.SetBool(StateParameterName.GetAnimatorHash(), true);

            // if (RootMotionOnEnter.HasValue)
            // {
            //     // if (RestoreRootMotionOnExit)
            //     // {
            //     //     oldRootMotion = animator.applyRootMotion;
            //     // }
            //
            //     animator.applyRootMotion = RootMotionOnEnter.Value;
            // }

            animator.SetInteger(AnimatorParameterId.RandomIndex.GetAnimatorHash(), 0);
            animator.SetInteger(AnimatorParameterId.LastIndex.GetAnimatorHash(), 0);
        }

        public virtual void Tick(ref MovementStateTransitionContext context)
        {
        }

        public virtual void Exit(ref MovementStateTransitionContext context)
        {
            var animator = context.Character.Animator;

            animator.SetBool(StateParameterName.GetAnimatorHash(), false);

            // if (RootMotionOnExit.HasValue)
            // {
            //     animator.applyRootMotion = RootMotionOnExit.Value;
            // }
            // else if (RestoreRootMotionOnExit)
            // {
            //     animator.applyRootMotion = oldRootMotion;
            // }
        }
    }
}