using UnityEngine;
using UnityEngine.Serialization;
using Yd.Collection;
using Yd.Gameplay.AbilitySystem;
using Yd.Gameplay.Object;
using Yd.Manager;

public class AbilitySystemInit : MonoBehaviour
{
    [SerializeField] private SKeyValuePair<GameplayEffectData, float>[] effectsDelayed;
    [FormerlySerializedAs("abilities")] [SerializeField] private SKeyValuePair<GameplayAbilityData, float>[] abilitiesDelayed;

    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();

        foreach (var (ability, delay) in abilitiesDelayed)
        {
            CoroutineTimer.SetTimer(_ => character.Controller.AbilitySystem.GrantAbility(ability), delay);
        }

        foreach (var (effect, delay) in effectsDelayed)
        {
            CoroutineTimer.SetTimer(_ => character.Controller.AbilitySystem.ApplyGameplayEffectAsync(effect, null), delay);
        }
    }
}