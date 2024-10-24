using UnityEngine;
using UnityEngine.Serialization;

namespace Yd.Animation
{
    public class AnimatorParameterSetter : StateMachineBehaviour
    {
        [FormerlySerializedAs("data")] [SerializeField] private AnimatorParameterConfig[] configs;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SetValues(animator, StateMachineBehaviourEvent.Enter);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SetValues(animator, StateMachineBehaviourEvent.Exit);
        }

        private void SetValues(Animator animator, StateMachineBehaviourEvent eventType)
        {
            foreach (var config in configs)
            {
                if (config.AnimatorParameterValues.TryGetValue(eventType, out var values))
                {
                    foreach (var animatorParameter in values)
                    {
                        animator.SetValue(animatorParameter);
                    }
                }

                if (config.setRootMotion && config.rootMotion.TryGetValue(eventType, out var rootMotion))
                {
                    animator.applyRootMotion = rootMotion;
                }
            }
        }
    }
}