using System;
using System.Collections.Generic;
using UnityEngine;
using Yd.Extension;

namespace Yd.Animation
{
    [Serializable]
    public enum AnimatorParameterId
    {
        None,

        // States
        Stand,
        Attack,
        Walk,
        Run,
        Jump,
        Fall,
        Hit,
        Stun,
        Die,

        // Random
        RandomIndex,
        RandomTransition,

        // Step
        StepLeft,
        StepRight,

        // obsolete
        StepLeftMiddle,
        StepRightMiddle,

        GroundVelocity,
        SpeedMagnitude,
        AttackId
    }

    [Serializable]
    public enum AnimatorParameterType
    {
        Bool,
        Int,
        Float
    }

    [Serializable]
    public struct AnimatorParameter
    {
        public AnimatorParameterId Id;
        public AnimatorParameterType Type;
        public float Value;
    }

    public static class AnimatorParameterExtension
    {
        private static Dictionary<AnimatorParameterId, int> parameterId;
        private static Dictionary<AnimatorParameterId, int> ParameterId =>
            parameterId ??= new Dictionary<AnimatorParameterId, int>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            // Debug.Log($"{nameof(AnimatorParameterExtension)} reset.");
            ParameterId?.Clear();
        }

        public static int GetAnimatorHash(this AnimatorParameterId parameter)
        {
            if (!ParameterId.TryGetValue(parameter, out var id))
            {
                id = Animator.StringToHash(parameter.GetString());
                ParameterId.Add(parameter, id);
            }

            // Debug.Log($"hash({parameter}) = {id}");

            return id;
        }
    }
}