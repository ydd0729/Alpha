using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public class AttributeModifierOperand
    {
        [SerializeField] public string tag;
        [SerializeField] public AttributeModifierOperandType type;
        [SerializeField] public float magnitude;
        [SerializeField] public AttributeSourceType attributeSourceType;
        [SerializeField] public GameplayAttributeTypeSO basedAttribute;

        public float Value(
            IReadOnlyDictionary<GameplayAttributeTypeSO, float> sourceAttributeValues,
            IReadOnlyDictionary<GameplayAttributeTypeSO, float> targetAttributeValues,
            IReadOnlyDictionary<string, float> taggedValues
        )
        {
            return type switch
            {
                AttributeModifierOperandType.Constant => magnitude,
                AttributeModifierOperandType.AttributeBased => attributeSourceType switch
                                                               {
                                                                   AttributeSourceType.Source => sourceAttributeValues?[
                                                                       basedAttribute] ??
                                                                   throw new NullReferenceException(),
                                                                   AttributeSourceType.Target => targetAttributeValues[
                                                                       basedAttribute],
                                                                   _ => throw new ArgumentOutOfRangeException()
                                                               } *
                                                               magnitude,
                AttributeModifierOperandType.SetByCaller => taggedValues[tag],
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void AddRequirements(
            List<GameplayAttributeTypeSO> requiredSourceAttributes, List<GameplayAttributeTypeSO> requiredTargetAttributes
        )
        {
            switch(attributeSourceType)
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