using System;
using Animation;
using UnityEngine;
using Yd.Audio;
using Yd.Extension;
using Yd.Gameplay;
using Yd.Gameplay.Object;

namespace Yd.Animation
{
    public sealed class AnimationEventListener : MonoBehaviour
    {
        public event Action<GameplayEventType> GameplayEventDispatcher;
        public event Action<string> AnimationEventDispatcher;

        private Character character;
        
        private void Awake()
        {
            character = GetComponent<Character>();
        }

        private void StepLeft()
        {
            GameplayEventDispatcher?.Invoke(GameplayEventType.StepLeft);
        }

        private void StepRight()
        {
            GameplayEventDispatcher?.Invoke(GameplayEventType.StepRight);
        }

        private void GameplayEvent(string @event)
        {
            GameplayEventDispatcher?.Invoke(@event.GetEnum<GameplayEventType>());
        }

        private void AnimationEvent(string @event)
        {
            switch(@event)
            {
                case AnimationEventType.Punch:
                    character.AudioManager.PlayOneShot(AudioId.Punch, AudioChannel.World);
                    break;
                case AnimationEventType.Kick:
                    character.AudioManager.PlayOneShot(AudioId.Kick, AudioChannel.World);
                    break;
                case AnimationEventType.BoarAttack:
                    character.AudioManager.PlayOneShot(AudioId.BoarAttack, AudioChannel.World);
                    break;
                case AnimationEventType.BoarRoar:
                    character.AudioManager.PlayOneShot(AudioId.BoarRoar, AudioChannel.World);
                    break;
                case AnimationEventType.HeroDie:
                    character.AudioManager.PlayOneShot(AudioId.HeroDie, AudioChannel.World);
                    break;
            }
            
            AnimationEventDispatcher?.Invoke(@event);
        }
    }
}