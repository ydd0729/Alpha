using UnityEngine;
using Yd.Audio;
using Yd.Extension;
using Yd.Gameplay.AbilitySystem;

namespace Yd.Gameplay.Object
{
    public class Actor : MonoBehaviour
    {
        private AudioManager audioManager;
        private GameplayAbilitySystem gameplayAbilitySystem;
        private GameplayTagSystem gameplayTagSystem;

        public GameplayAbilitySystem AbilitySystem =>
            gameplayAbilitySystem ??= gameObject.GetComponent<GameplayAbilitySystem>();
        public GameplayTagSystem Tags => gameplayTagSystem ??= gameObject.GetComponent<GameplayTagSystem>();
        public AudioManager AudioManager => audioManager ??= gameObject.GetOrAddComponent<AudioManager>();
    }
}