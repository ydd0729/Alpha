using System.Collections.Generic;
using UnityEngine;
using Yd.Collection;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu
        (fileName = "Gameplay Attributes", menuName = "Scriptable Objects/Gameplay Ability System/Gameplay Attributes")]
    public class GameplayAttributes : ScriptableObject
    {
        [SerializeField] private SDictionary<GameplayAttributeTypeEnum, GameplayAttributeTypeSO> attrributes;

        public IReadOnlyDictionary<GameplayAttributeTypeEnum, GameplayAttributeTypeSO> Attrributes => attrributes;
    }
}