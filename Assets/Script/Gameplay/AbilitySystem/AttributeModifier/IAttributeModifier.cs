using System.Collections.Generic;
using JetBrains.Annotations;

namespace Yd.Gameplay.AbilitySystem
{
    public interface IAttributeModifier
    {
        void Calculate(
            [CanBeNull] IReadOnlyDictionary<GameplayAttributeTypeSO, float> sourceAttributeValues,
            IReadOnlyDictionary<GameplayAttributeTypeSO, float> targetAttributeValues,
            IDictionary<GameplayAttributeTypeSO, float> moddingAttributeValues
        );
    }
}