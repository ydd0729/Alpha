using UnityEngine;

namespace Yd.Gameplay.AbilitySystem
{
    public abstract class ComboAbilityData : GameplayAbilityData
    {
        [SerializeField] private int maxCombo;
        public int MaxCombo => maxCombo;
    }
}