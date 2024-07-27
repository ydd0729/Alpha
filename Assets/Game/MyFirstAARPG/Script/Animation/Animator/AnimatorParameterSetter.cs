using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyFirstAARPG
{
    public class AnimatorParameterSetter : StateMachineBehaviour
    {
        [SerializeField] private AnimatorParameterSetterData Data;

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
            if (Data.AnimatorParameterValues.TryGetValue(eventType, out var values))
            {
                foreach (var KV in values)
                {
                    animator.SetValue(new AnimatorParameter {Name = KV.Key, Value = KV.Value});
                }
            }
        }
    }
}