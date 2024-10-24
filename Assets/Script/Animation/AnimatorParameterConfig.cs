using UnityEngine;
using Yd.Collection;

namespace Yd.Animation
{
    [CreateAssetMenu
        (fileName = "Animator Parameter Setter Data", menuName = "Scriptable Objects/Animation/Animator Parameter Setter Data")]
    public class AnimatorParameterConfig : ScriptableObject
    {
        [SerializeField]
        private SDictionary<StateMachineBehaviourEvent, AnimatorParameter[]> animatorParameterValues;

        [SerializeField] public bool setRootMotion;
        [SerializeField] public SDictionary<StateMachineBehaviourEvent, bool> rootMotion;

        public SDictionary<StateMachineBehaviourEvent, AnimatorParameter[]> AnimatorParameterValues =>
            animatorParameterValues;
    }
}