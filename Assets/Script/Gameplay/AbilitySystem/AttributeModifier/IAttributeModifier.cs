using System.Collections.Generic;
using JetBrains.Annotations;

namespace Yd.Gameplay.AbilitySystem
{
    public interface IAttributeModifier
    {
        void Calculate(
            [CanBeNull] IReadOnlyDictionary<GameplayAttributeType, float> sourceAttributeValues,
            IReadOnlyDictionary<GameplayAttributeType, float> targetAttributeValues,
            IDictionary<GameplayAttributeType, float> moddingAttributeValues
        );
    }
}