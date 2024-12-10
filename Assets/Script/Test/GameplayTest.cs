using UnityEngine;
using Yd.Collection;
using Yd.Gameplay.AbilitySystem;
using Yd.Gameplay.Object;
using Yd.Manager;

public class GameplayTest : MonoBehaviour
{
    [SerializeField] private SKeyValuePair<GameplayEffectData, float>[] effectsDelayed;
    [SerializeField] private SKeyValuePair<GameplayAbilityData, float>[] abilities;

    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();

        foreach (var (ability, delay) in abilities)
        {

            CoroutineTimer.SetTimer(_ => character.Controller.AbilitySystem.GrantAbility(ability), delay);
        }

        foreach (var (effect, delay) in effectsDelayed)
        {
            CoroutineTimer.SetTimer(_ => character.Controller.AbilitySystem.ApplyGameplayEffectAsync(effect, null), delay);
        }
    }
}