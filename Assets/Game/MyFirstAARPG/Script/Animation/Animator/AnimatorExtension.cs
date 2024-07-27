using UnityEngine;

namespace MyFirstAARPG
{
    public static class AnimatorExtension
    {
        public static void SetValue(this Animator animator, AnimatorParameter parameter)
        {
            switch (parameter.Value.ValueType)
            {
                case AnimatorParameterType.Bool:
                {
                    animator.SetBool(parameter.Name.GetId(), parameter.Value.BoolValue);
                    break;
                }
                case AnimatorParameterType.Integer:
                {
                    animator.SetInteger(parameter.Name.GetId(), parameter.Value.IntegerValue);
                    break;
                }
            }
        }
    }
}