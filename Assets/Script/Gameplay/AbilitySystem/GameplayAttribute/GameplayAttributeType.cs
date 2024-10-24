using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Yd.Collection;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu
        (fileName = "Gameplay Attribute Type", menuName = "Scriptable Objects/Gameplay Ability System/Gameplay Attribute Type")]
    public class GameplayAttributeType : ScriptableObject
    {
        [FormerlySerializedAs("validRange")] [SerializeField] private SRangeFloat range = new(0, 999999);
        [SerializeField] [CanBeNull] private GameplayAttributeType MaxAttribute;

        public SRange<float> Range => range;
    }
}