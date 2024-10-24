using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public class AttributeModifierOperand
    {
        [SerializeField] public AttributeModifierOperandType type;
        [SerializeField] public float magnitude;
        [SerializeField] public AttributeSourceType attributeSourceType;
        [SerializeField] public GameplayAttributeType basedAttribute;

        public float Value(IReadOnlyDictionary<GameplayAttributeType, float> sourceAttributeValues, IReadOnlyDictionary<GameplayAttributeType, float> targetAttributeValues)
        {
            return type switch
            {
                AttributeModifierOperandType.Constant => magnitude,
                AttributeModifierOperandType.AttributeBased => attributeSourceType switch
                {
                    AttributeSourceType.Source => sourceAttributeValues?[basedAttribute] ?? throw new NullReferenceException(),
                    AttributeSourceType.Target => targetAttributeValues[basedAttribute],
                    _ => throw new ArgumentOutOfRangeException()
                } * magnitude,
                AttributeModifierOperandType.SetByCaller => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void AddRequirements(List<GameplayAttributeType> requiredSourceAttributes, List<GameplayAttributeType> requiredTargetAttributes)
        {
            switch (attributeSourceType)
            {
                case AttributeSourceType.Source:
                    if (!requiredSourceAttributes.Contains(basedAttribute))
                    {
                        requiredSourceAttributes.Add(basedAttribute);
                    }

                    break;
                case AttributeSourceType.Target:
                    if (!requiredTargetAttributes.Contains(basedAttribute))
                    {
                        requiredTargetAttributes.Add(basedAttribute);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}