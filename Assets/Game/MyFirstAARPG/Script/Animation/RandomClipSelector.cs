using System;
using Shared.Collections;
using Shared.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyFirstAARPG
{
    public class RandomClipSelector : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator characterAnimator;

        private void Start()
        {
            characterAnimator.AnimationStateChanged += CharacterAnimator_OnAnimationStateChanged();
        }
        
        private Coroutine timerHandle;

        private EventHandler<AnimationStateChangedEventArgs> CharacterAnimator_OnAnimationStateChanged()
        {
            return (_, AnimationState) =>
            {
                if (!characterAnimator.Data.RandomIndexPolicies.TryGetValue(AnimationState.CurrentStateParameterName,
                        out var randomIndexPolicy))
                {
                    return;
                }

                CoroutineTimer.Cancel(ref timerHandle);

                timerHandle = CoroutineTimer.SetTimer(
                    () =>
                    {
                        int index = Random.Range(randomIndexPolicy.MinInclusiveIndex, randomIndexPolicy.MaxExclusiveIndex);
                        if (!randomIndexPolicy.CanDuplicate)
                        {
                            int lastIndex =
                                characterAnimator.Animator.GetInteger(AnimatorParameterName.LastIndex.GetId());
                            
                            while (lastIndex == index)
                            {
                                index = Random.Range(randomIndexPolicy.MinInclusiveIndex, randomIndexPolicy.MaxExclusiveIndex);
                            }
                            
                            characterAnimator.Animator.SetInteger(AnimatorParameterName.LastIndex.GetId(), index);
                        }

                        characterAnimator.Animator.SetInteger(AnimatorParameterName.RandomIndex.GetId(), index);
                    },
                    randomIndexPolicy.minDuration, randomIndexPolicy.maxDuration, randomIndexPolicy.LoopPolicy);
            };
        }
    }
}