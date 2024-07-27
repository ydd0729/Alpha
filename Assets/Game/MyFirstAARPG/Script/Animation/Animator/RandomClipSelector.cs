using System;
using Shared.Collections;
using Shared.Manager;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MyFirstAARPG
{
    public class RandomClipSelector : MonoBehaviour
    {
        [FormerlySerializedAs("characterAnimator")] [SerializeField] private CharacterAnimation characterAnimation;

        private void Start()
        {
            characterAnimation.AnimationStateChanged += OnAnimationStateChanged;
        }

        private Coroutine timerHandle;

        private void OnAnimationStateChanged(AnimationStateChangedEventArgs args)
        {
            if (!characterAnimation.Data.RandomIndexPolicies.TryGetValue(args.CurrentStateParameterName, out var randomIndexPolicy))
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
                        int lastIndex = characterAnimation.Animator.GetInteger(AnimatorParameterName.LastIndex.GetId());

                        while (lastIndex == index)
                        {
                            index = Random.Range(randomIndexPolicy.MinInclusiveIndex, randomIndexPolicy.MaxExclusiveIndex);
                        }

                        characterAnimation.Animator.SetInteger(AnimatorParameterName.LastIndex.GetId(), index);
                    }

                    characterAnimation.Animator.SetInteger(AnimatorParameterName.RandomIndex.GetId(), index);
                },
                randomIndexPolicy.minDuration, randomIndexPolicy.maxDuration, randomIndexPolicy.LoopPolicy);
        }
    }
}