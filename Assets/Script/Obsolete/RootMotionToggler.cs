using UnityEngine;
using UnityEngine.Serialization;

namespace Yd
{
    public class RootMotionToggler : StateMachineBehaviour
    {
        [FormerlySerializedAs("Data")] [SerializeField] private RootMotionTogglerData data;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (data.ToggleRootMotion.TryGetValue(StateMachineBehaviourEvent.Enter, out var value))
            {
                animator.applyRootMotion = value;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (data.ToggleRootMotion.TryGetValue(StateMachineBehaviourEvent.Exit, out var value))
            {
                animator.applyRootMotion = value;
            }
        }
    }

}