using System;
using UnityEngine.Serialization;

namespace Yd.Gameplay.AbilitySystem
{
    [Serializable]
    public struct GameplayAttributeValue
    {
        [FormerlySerializedAs("Type")] public GameplayAttributeType type;
        [FormerlySerializedAs("Value")] public float value;

        public GameplayAttributeValue(GameplayAttributeType type, float value)
        {
            this.type = type;
            this.value = value;
        }

        internal void Deconstruct(out GameplayAttributeType type, out float value)
        {
            type = this.type;
            value = this.value;
        }
    }
}