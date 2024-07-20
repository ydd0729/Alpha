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
        // [SerializeField]
        // private List<KeyValuePair_StateMachineBehaviourEvents_AnimatorParameterValue> animatorParameterValueList;
        //
        // private void OnValidate()
        // {
        //     if (animatorParameterValueList == null)
        //     {
        //         return;
        //     }
        //
        //     AnimatorParameterValues.Clear();
        //
        //     foreach (var KV in animatorParameterValueList)
        //     {
        //         if (!AnimatorParameterValues.ContainsKey(KV.Key))
        //         {
        //             AnimatorParameterValues.Add(KV.Key, new());
        //         }
        //
        //         if (!AnimatorParameterValues[KV.Key].TryAdd(KV.Value.Name, KV.Value))
        //         {
        //             Debug.LogWarning($"the AnimatorParameterValue {KV.Value} already exists when {KV.Key}.");
        //         }
        //     }
        // }

        [SerializeField]
        private SerializableDictionary<StateMachineBehaviourEvent, SerializableDictionary<AnimatorParameterName, AnimatorParameterValue>> animatorParameterValues;

        public SerializableDictionary<StateMachineBehaviourEvent, SerializableDictionary<AnimatorParameterName, AnimatorParameterValue>> AnimatorParameterValues =>
            animatorParameterValues ??= new();
    }

    // [Serializable]
    // public struct KeyValuePair_StateMachineBehaviourEvents_AnimatorParameterValue
    // {
    //     public StateMachineBehaviourEvent Key;
    //     public AnimatorParameterValue Value;
    // }
}