using System;
using UnityEngine;
using UnityEngine.Serialization;
using Yd.Animation;
using Yd.Collection;
using Yd.Manager;

namespace Yd.Gameplay.Object
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/Gameplay/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [FormerlySerializedAs("ControllerPrefab")]
        public GameObject controllerPrefab;

        [FormerlySerializedAs("AnimatorRandomization")]
        public SDictionary<AnimatorParameterId, AnimatorRandomizationConfig> animatorRandomization;
    }

    [Serializable]
    public struct AnimatorRandomizationConfig
    {
        [FormerlySerializedAs("loopPolicy")] [FormerlySerializedAs("LoopPolicy")]
        public CoroutineTimerLoopPolicy coroutineTimerLoopPolicy;

        [FormerlySerializedAs("DurationRange")]
        public SRange<float> durationRange;

        [FormerlySerializedAs("CanDuplicate")] public bool canDuplicate;

        [FormerlySerializedAs("IndexRange")] public SRangeInt indexRange;
    }
}