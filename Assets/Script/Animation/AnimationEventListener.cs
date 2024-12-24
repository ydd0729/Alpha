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

        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
        }
        public event Action<GameplayEventArgs> GameplayEventDispatcher;
        public event Action<string> AnimationEventDispatcher;

        private void StepLeft()
        {
            GameplayEventDispatcher?.Invoke(new GameplayEventArgs { EventType = GameplayEventType.StepLeft });
        }

        private void StepRight()
        {
            GameplayEventDispatcher?.Invoke(new GameplayEventArgs { EventType = GameplayEventType.StepRight });
        }

        private void GameplayEvent(string @event)
        {
            GameplayEventDispatcher?.Invoke(new GameplayEventArgs { EventType = @event.GetEnum<GameplayEventType>() });
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
                case AnimationEventType.LavaAttack:
                    character.AudioManager.PlayOneShot(AudioId.LavaAttack, AudioChannel.Lava);
                    break;
                case AnimationEventType.LavaDie:
                    character.AudioManager.PlayOneShot(AudioId.LavaDie, AudioChannel.Lava);
                    break;
                case AnimationEventType.SwordSwing:
                    character.AudioManager.PlayOneShot(AudioId.SwordSwing, AudioChannel.World);
                    break;
            }

            AnimationEventDispatcher?.Invoke(@event);
        }
    }
}