using UnityEngine;

namespace Yd.Gameplay.AbilitySystem
{
    [CreateAssetMenu
    (
        fileName = "Resilience Recovery Ability",
        menuName = "Scriptable Objects/Gameplay/Gameplay Ability System/Resilience Recovery Ability"
    )]
    public class ResilienceRecoveryAbilityData : GameplayAbilityData
    {
        [SerializeField] private GameplayEffectData recoverEffect;
        [SerializeField] private float recoverDelay;

        public GameplayEffectData RecoverEffect => recoverEffect;
        public float RecoverDelay => recoverDelay;

        public override GameplayAbility Create(GameplayAbilitySystem owner, GameplayAbilitySystem source)
        {
            return new ResilienceRecoveryAbility(this, owner, source);
        }
    }
}