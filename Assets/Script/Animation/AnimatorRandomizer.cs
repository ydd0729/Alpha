using UnityEngine;
using Yd;
using Yd.Algorithm;
using Yd.Animation;
using Yd.Collection;
using Yd.Manager;

namespace Script.Animation
{
    public class AnimatorRandomizer : StateMachineBehaviour
    {
        [SerializeField] private SRangeInt valueRange;
        [SerializeField] private SRangeInt periodRange;
        [SerializeField] private int minDuplicateGap;
        [SerializeField] private bool isInfinite;
        private int? layerIndex;

        private Coroutine timer;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (isInfinite)
            {
                this.layerIndex ??= layerIndex;
                animator.SetValue(AnimatorParameterId.RandomTransition, false);
            }
        }

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineEnter(animator, stateMachinePathHash);

            animator.SetValue(AnimatorParameterId.RandomIndex, 0);
            var randomInt = new RandomInt(valueRange.Min, valueRange.Max, minDuplicateGap);

            if (isInfinite)
            {
                timer = CoroutineTimer.SetTimer
                (
                    _ => {
                        if (layerIndex.HasValue && !animator.IsInTransition(layerIndex.Value))
                        {
                            animator.SetValue(AnimatorParameterId.RandomIndex, randomInt.Next());
                            animator.SetValue(AnimatorParameterId.RandomTransition, true);
                        }
                    },
                    periodRange.Min,
                    periodRange.Max,
                    CoroutineTimerLoopPolicy.InfiniteLoop
                );
            }
            else
            {
                animator.SetValue(AnimatorParameterId.RandomIndex, randomInt.Next());
                animator.SetValue(AnimatorParameterId.RandomTransition, true);
            }

        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineExit(animator, stateMachinePathHash);

            CoroutineTimer.Cancel(ref timer);
        }
    }
}