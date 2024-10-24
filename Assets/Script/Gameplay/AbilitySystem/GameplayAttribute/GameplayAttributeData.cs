using UnityEngine;
using Yd.Collection;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu(fileName = "Gameplay Attribute Data", menuName = "Scriptable Objects/Gameplay Ability System/Gameplay Attribute Data")]
    public class GameplayAttributeData : ScriptableObject
    {
        [SerializeField] public SDictionary<GameplayAttributeType, float> Data;
    }
}