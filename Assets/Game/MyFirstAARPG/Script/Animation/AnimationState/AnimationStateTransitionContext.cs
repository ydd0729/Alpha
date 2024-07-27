using UnityEngine;

namespace MyFirstAARPG
{
    public struct AnimationStateTransitionContext
    {
        public Animator Animator;
        public AnimationState CurrentState;
        public bool IsInAir;
    }
}