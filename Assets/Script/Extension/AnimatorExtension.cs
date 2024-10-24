using System;
using UnityEngine;
using Yd.Animation;

namespace Yd
{
    public static class AnimatorE
    {
        public static void SetValue(this Animator animator, AnimatorParameter animatorParameter)
        {
            switch(animatorParameter.Type)
            {

                case AnimatorParameterType.Bool:
                    animator.SetValue(animatorParameter.Id, animatorParameter.Value != 0);
                    break;
                case AnimatorParameterType.Int:
                    animator.SetValue(animatorParameter.Id, (int)animatorParameter.Value);

                    break;
                case AnimatorParameterType.Float:
                    animator.SetValue(animatorParameter.Id, animatorParameter.Value);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void SetValue(this Animator animator, AnimatorParameterId name, int value)
        {
            animator.SetInteger(name.GetAnimatorHash(), value);
        }

        public static void SetValue(this Animator animator, AnimatorParameterId name, bool value)
        {
            animator.SetBool(name.GetAnimatorHash(), value);
        }

        public static void SetValue(this Animator animator, AnimatorParameterId name, float value)
        {
            animator.SetFloat(name.GetAnimatorHash(), value);
        }
    }
}