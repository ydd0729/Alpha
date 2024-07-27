using Shared.Collections;
using UnityEngine;

namespace Shared.Gameplay.AbilitySystem
{
    public class GameplayAttributeInitialValues : ScriptableObject
    {
        [SerializeField] public SerializableDictionary<GameplayAttributeType, float> Data;
    }
}