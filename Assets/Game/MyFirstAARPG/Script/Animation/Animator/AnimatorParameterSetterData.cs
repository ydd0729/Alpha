using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Collections;
using UnityEngine;

namespace MyFirstAARPG
{
    [CreateAssetMenu(fileName = "Animator Parameter Setter Data", menuName = "Scriptable Objects/Animator Parameter Setter Data")]
    public class AnimatorParameterSetterData : ScriptableObject
    {
        [SerializeField]
        private SerializableDictionary<StateMachineBehaviourEvent, SerializableDictionary<AnimatorParameterName, AnimatorParameterValue>> 
            animatorParameterValues;

        public SerializableDictionary<StateMachineBehaviourEvent, SerializableDictionary<AnimatorParameterName, AnimatorParameterValue>> 
            AnimatorParameterValues => animatorParameterValues ??= new();
    }
}