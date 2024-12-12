using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Yd.Collection;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu
        (fileName = "Gameplay Attribute Type", menuName = "Scriptable Objects/Gameplay Ability System/Gameplay Attribute Type")]
    public class GameplayAttributeTypeSO : ScriptableObject
    {
        [FormerlySerializedAs("validRange")] [SerializeField] private SRangeFloat range = new(0, 999999);
        [FormerlySerializedAs("MaxAttribute")] [SerializeField] [CanBeNull] private GameplayAttributeTypeSO maxAttribute;
        [SerializeField] [CanBeNull] private GameplayAttributeTypeSO valueAttribute;
        [SerializeField] private GameplayAttributeTypeEnum type;

        public SRange<float> Range => range;
        public GameplayAttributeTypeEnum Type => type;
        public GameplayAttributeTypeSO MaxAttribute => maxAttribute;
        public GameplayAttributeTypeSO ValueAttribute => valueAttribute;
    }

    public enum GameplayAttributeTypeEnum
    {
        Health,
        MaxHealth,
        Resilience
    }
}