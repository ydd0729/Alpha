using System;
using System.Collections.Generic;
using Shared.Collections;
using UnityEngine;

namespace Shared.Gameplay.AbilitySystem
{
    // [Serializable]
    public struct GameplayAttribute
    {
        // [SerializeField]
        public readonly GameplayAttributeType AttributeType;

        public GameplayAttribute(GameplayAttributeType attributeType)
        {
            AttributeType = attributeType;
        }

        public GameplayAttributeData GetGameplayAttributeData(GameplayAttributeSet Src)
        {
            return Src.Attributes.GetValueOrDefault(AttributeType);
        }
    }
}