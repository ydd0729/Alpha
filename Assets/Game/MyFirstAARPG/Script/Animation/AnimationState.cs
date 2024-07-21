using Shared.Collections;
using Shared.Pattern;

namespace MyFirstAARPG
{
    public abstract class AnimationState : IState
    {
        protected AnimationState(CharacterAnimator characterAnimator)
        {
            this.characterAnimator = characterAnimator;
        }
        
        public abstract AnimatorParameterName StateParameterName { get; }
        
        public virtual void Enter()
        {
            characterAnimator.Animator.SetBool(StateParameterName.GetId(), true);
            // characterAnimator.Animator.applyRootMotion = ApplyRootMotion;
            
            characterAnimator.Animator.SetInteger(AnimatorParameterName.RandomIndex.GetId(), 0);
            characterAnimator.Animator.SetInteger(AnimatorParameterName.LastIndex.GetId(), 0);
        }

        public virtual void Exit()
        {
            characterAnimator.Animator.SetBool(StateParameterName.GetId(), false);
            // characterAnimator.Animator.applyRootMotion = false;
        }
        
        protected readonly CharacterAnimator characterAnimator;
        // protected abstract bool ApplyRootMotion { get; }
    }
    
    public class StandState : AnimationState
    {
        public StandState(CharacterAnimator characterAnimator) : base(characterAnimator)
        {
        }
        
        public override AnimatorParameterName StateParameterName => AnimatorParameterName.Stand;

        // protected override bool ApplyRootMotion => false;
    }

    public class WalkState : AnimationState
    {
        public WalkState(CharacterAnimator characterAnimator) : base(characterAnimator)
        {
        }
        
        public override AnimatorParameterName StateParameterName => AnimatorParameterName.Walk;

        // protected override bool ApplyRootMotion => true;
    }
    
    public class RunState : AnimationState
    {
        public RunState(CharacterAnimator characterAnimator) : base(characterAnimator)
        {
        }
        
        public override AnimatorParameterName StateParameterName => AnimatorParameterName.Run;

        // protected override bool ApplyRootMotion => true;
    }
}