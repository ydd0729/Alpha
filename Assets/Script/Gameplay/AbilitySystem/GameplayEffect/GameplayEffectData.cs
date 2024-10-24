using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu
        (fileName = "Gameplay Effect", menuName = "Scriptable Objects/Gameplay/Gameplay Ability System/Gameplay Effect")]
    public class GameplayEffectData : ScriptableObject
    {
        [SerializeField] private GameplayEffectType type;

        [SerializeField] private float duration;

        [SerializeField] private float period;

        [SerializeField] private GEChannel channel;

        [FormerlySerializedAs("checkRange")]
        [SerializeField] private bool allModsMustValid; // 超过 Attribute Type 定义的 Range 的任何修改都不会生效。

        [SerializeField] private AttributeModifier[] attributeModifiers;

        public GameplayEffectType Type => type;

        public IEnumerable<AttributeModifier> AttributeModifiers => attributeModifiers;

        public float Duration => duration;
        public float Period => period;

        public bool AllModsMustValid => allModsMustValid;

        public bool IsPeriodic => period != 0;

        public GEChannel Channel => channel;

        // private void OnValidate()
        // {
        //     foreach (var modifier in attributeModifiers)
        //     {
        //         modifier.OnValidate();
        //     }
        // }
    }
}