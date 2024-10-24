using UnityEngine;
using Yd.Animation;
using Yd.Manager;

namespace Yd.Gameplay.Object
{
    public class CharacterAnimationRandomizer : MonoBehaviour
    {
        private Character Character;
        private Coroutine timerHandle;

        public void Initialize(Character character)
        {
            Character = character;
            character.Movement.MovementStateChanged += OnMovementStateChanged;
        }

        private void OnMovementStateChanged(MovementStateChangedEventArgs args)
        {
            if (!Character.Data.animatorRandomization.TryGetValue(args.CurrentStateParameterName, out var config))
            {
                return;
            }

            CoroutineTimer.Cancel(ref timerHandle);

            timerHandle = CoroutineTimer.SetTimer
            (
                _ => {
                    var index = config.indexRange.Random();
                    if (!config.canDuplicate)
                    {
                        var lastIndex = Character.Animator.GetInteger(AnimatorParameterId.LastIndex.GetAnimatorHash());

                        while (lastIndex == index)
                        {
                            index = config.indexRange.Random();
                        }

                        Character.Animator.SetValue(AnimatorParameterId.LastIndex, index);
                    }

                    Character.Animator.SetValue(AnimatorParameterId.RandomIndex, index);
                },
                config.durationRange.Min,
                config.durationRange.Max,
                config.coroutineTimerLoopPolicy
            );
        }
    }
}