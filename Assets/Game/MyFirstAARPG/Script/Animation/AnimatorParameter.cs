using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyFirstAARPG
{
    [Serializable]
    public enum AnimatorParameterName
    {
        None,
        Stand,
        Walk,
        Run,
        RandomIndex,
        LastIndex,
        StepLeft,
        StepRight,
        StepLeftMiddle,
        StepRightMiddle,
    }

    [Serializable]
    public enum AnimatorParameterType
    {
        None,
        Bool,
        Integer,
        // Copy
    }

    [Serializable]
    public struct AnimatorParameterValue
    {
        // public AnimatorParameterName Name;
        public AnimatorParameterType ValueType;
        public bool BoolValue;
        public int IntegerValue;
        // public AnimatorParameterName CopyFrom;
    }
    
    [Serializable]
    public struct AnimatorParameter
    {
        public AnimatorParameterName Name;
        public AnimatorParameterValue Value;
    }

    public static class AnimatorParameterExtension
    {
        public static string GetString(this AnimatorParameterName parameterName)
        {
            if (!ParameterStr.ContainsKey(parameterName))
            {
                ParameterStr.Add(parameterName, parameterName.ToString());
            }

            return ParameterStr[parameterName];
        }

        public static int GetId(this AnimatorParameterName parameterName)
        {
            if (!ParameterId.ContainsKey(parameterName))
            {
                ParameterId.Add(parameterName, Animator.StringToHash(parameterName.GetString()));
            }

            return ParameterId[parameterName];
        }

        // public static AnimatorParameterType GetParameterType(this AnimatorParameterName parameterName)
        // {
        //     switch (parameterName)
        //     {
        //         case AnimatorParameterName.Stand:
        //         case AnimatorParameterName.Walk:
        //         case AnimatorParameterName.StepLeft:
        //         case AnimatorParameterName.StepRight:
        //             return AnimatorParameterType.Bool;
        //         case AnimatorParameterName.RandomIndex:
        //         case AnimatorParameterName.LastIndex:
        //             return AnimatorParameterType.Integer;
        //         default: return AnimatorParameterType.None;
        //     }
        // }

        private static Dictionary<AnimatorParameterName, int> ParameterId => parameterId ??= new();
        private static Dictionary<AnimatorParameterName, string> ParameterStr => parameterStr ??= new();

        private static Dictionary<AnimatorParameterName, int> parameterId;
        private static Dictionary<AnimatorParameterName, string> parameterStr;
    }
}