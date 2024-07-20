using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace MyFirstAARPG
{
    public class RootMotionToggler : StateMachineBehaviour
    {
        [SerializeField] private RootMotionTogglerData Data;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Data.ToggleRootMotion.ContainsKey(StateMachineBehaviourEvent.Enter))
            {
                animator.applyRootMotion = Data.ToggleRootMotion[StateMachineBehaviourEvent.Enter];
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Data.ToggleRootMotion.ContainsKey(StateMachineBehaviourEvent.Exit))
            {
                animator.applyRootMotion = Data.ToggleRootMotion[StateMachineBehaviourEvent.Exit];
            }
        }
    }

}