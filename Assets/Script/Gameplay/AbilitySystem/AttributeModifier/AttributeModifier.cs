using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public class AttributeModifier
    {
        [SerializeField] private GameplayAttributeType targetAttribute;
        [FormerlySerializedAs("attributeModifierOperations")]
        [SerializeField] protected List<AttributeModifierOperation> operations;
        // [SerializeField] protected List<GameplayAttributeType> requiredSourceAttributes;
        // [SerializeField] protected List<GameplayAttributeType> requiredTargetAttributes;

        public void Calculate(
            [CanBeNull] IReadOnlyDictionary<GameplayAttributeType, float> sourceAttributeValues,
            IReadOnlyDictionary<GameplayAttributeType, float> targetAttributeValues,
            IDictionary<GameplayAttributeType, float> moddingAttributeValues
        )
        {
            moddingAttributeValues.TryAdd(targetAttribute, targetAttributeValues[targetAttribute]);

            var mod = moddingAttributeValues[targetAttribute];

            foreach (var (@operator, operand) in operations)
            {
                var operandValue = operand.Value(sourceAttributeValues, targetAttributeValues);

                switch(@operator)
                {
                    case AttributeModifierOperator.Set:
                        mod = operandValue;
                        break;
                    case AttributeModifierOperator.Modify:
                        mod += operandValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            moddingAttributeValues[targetAttribute] = mod;
        }
    }
}