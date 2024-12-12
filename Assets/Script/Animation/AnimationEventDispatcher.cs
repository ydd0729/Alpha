using System;
using UnityEngine;
using Yd.Extension;
using Yd.Gameplay;

namespace Yd.Animation
{
    public sealed class AnimationEventDispatcher : MonoBehaviour
    {
        public event Action<GameplayEvent> Event;

        private void StepLeft()
        {
            Event?.Invoke(Gameplay.GameplayEvent.StepLeft);
        }

        private void StepRight()
        {
            Event?.Invoke(Gameplay.GameplayEvent.StepRight);
        }

        private void StepLeftMiddle()
        {
            Event?.Invoke(Gameplay.GameplayEvent.StepLeftMiddle);
        }

        private void StepRightMiddle()
        {
            Event?.Invoke(Gameplay.GameplayEvent.StepRightMiddle);
        }

        private void GameplayEvent(string @event)
        {
            Event?.Invoke(@event.GetEnum<GameplayEvent>());
        }
    }
}