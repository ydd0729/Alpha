using System;
using UnityEngine;
using UnityEngine.Events;
using Yd.Extension;
using Yd.Gameplay;

namespace Yd.Animation
{
    public sealed class AnimationEventDispatcher : MonoBehaviour
    {
        public event UnityAction<AnimationEvent> Step;
        public event Action<GameplayEvent> Gameplay;

        // binding functions

        private void StepLeft()
        {
            Step?.Invoke(AnimationEvent.StepLeft);
        }

        private void StepRight()
        {
            Step?.Invoke(AnimationEvent.StepRight);
        }

        private void StepLeftMiddle()
        {
            Step?.Invoke(AnimationEvent.StepLeftMiddle);
        }

        private void StepRightMiddle()
        {
            Step?.Invoke(AnimationEvent.StepRightMiddle);
        }

        private void GameplayEvent(string @event)
        {
            Gameplay?.Invoke(@event.GetEnum<GameplayEvent>());
        }
    }
}