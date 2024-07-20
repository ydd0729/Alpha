using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Collections;
using Shared.Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyFirstAARPG
{
    [CreateAssetMenu(fileName = "Animator Data", menuName = "Scriptable Objects/Animator Data")]
    public class AnimatorData : ScriptableObject
    {
        [SerializeField]
        private SerializableDictionary<AnimatorParameterName, RandomIndexPolicy> randomIndexPolicies;

        public SerializableDictionary<AnimatorParameterName, RandomIndexPolicy> RandomIndexPolicies => randomIndexPolicies ??= new();
    }

    [Serializable]
    public struct RandomIndexPolicy
    {
        public LoopPolicy LoopPolicy;
        [FormerlySerializedAs("MinIndex")] public int MinInclusiveIndex;
        [FormerlySerializedAs("MaxIndex")] public int MaxExclusiveIndex;
        public bool CanDuplicate;
        public float minDuration;
        public float maxDuration;
    }
}

