using System;
using UnityEngine.Serialization;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public struct GameplayAttributeValue
    {
        [FormerlySerializedAs("type")] [FormerlySerializedAs("Type")] public GameplayAttributeTypeSO typeSo;
        [FormerlySerializedAs("Value")] public float value;

        public GameplayAttributeValue(GameplayAttributeTypeSO typeSo, float value)
        {
            this.typeSo = typeSo;
            this.value = value;
        }

        internal void Deconstruct(out GameplayAttributeTypeSO typeSo, out float value)
        {
            typeSo = this.typeSo;
            value = this.value;
        }
    }
}