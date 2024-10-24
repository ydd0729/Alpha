using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;
using Yd.Manager;

namespace Yd.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private readonly Dictionary<AudioId, IEnumerator<AudioItem>> audioContainerEnumerators = new();
        private readonly Dictionary<GameObject, ObjectPool<AudioSource>> audioSourcePool = new();

        private void Awake()
        {
            Clear();
        }

        private void OnDestroy()
        {
            Clear();
        }

        private void Clear()
        {
            audioContainerEnumerators.Clear();
            audioSourcePool.Clear();
        }

        public void PlayOneShot(AudioId audioId, AudioChannel channel, [CanBeNull] GameObject target = null)
        {
            if (target == null)
            {
                target = gameObject;
            }

            var audioSource = audioSourcePool.GetValueOrAdd
            (
                target,
                new ObjectPool<AudioSource>
                (
                    () => {
                        {
                            var audioSource = target.AddComponent<AudioSource>();
                            audioSource.playOnAwake = false;
                            audioSource.enabled = false;
                            return audioSource;
                        }
                    },
                    OnGetFromPool,
                    OnReleaseToPool,
                    OnDestroyPooled,
                    true,
                    1,
                    10
                )
            ).Get();

            if (audioContainerEnumerators.TryGetValue(audioId, out var enumerator))
            {
                SetAudioItem(audioSource, enumerator);
            }
            else
            {
                switch( GlobalData.Instance.Audio.AudioResources[audioId] )
                {
                    case AudioClip clip:
                        audioSource.clip = clip;
                        break;
                    case AudioContainer audioContainer:
                        enumerator = audioContainer.GetEnumerator();
                        SetAudioItem(audioSource, enumerator);
                        audioContainerEnumerators[audioId] = enumerator;
                        break;
                }
            }

            audioSource.outputAudioMixerGroup = GlobalData.Instance.Audio.AudioMixerGroups[channel];

            audioSource.Play();

            CoroutineTimer.SetTimer
            (
                _ => {
                    if (!audioSource.isPlaying)
                    {
                        audioSourcePool[target].Release(audioSource);
                    }
                },
                audioSource.clip.length + 0.1f
            );
        }

        private static void SetAudioItem(AudioSource audioSource, IEnumerator<AudioItem> enumerator)
        {
            if (!enumerator.MoveNext())
            {
                enumerator.Reset();
            }

            var audioItem = enumerator.Current;

            audioSource.volume = audioItem.volume;
            audioSource.pitch = audioItem.pitch;
            audioSource.clip = audioItem.audioClip;
            audioSource.spatialBlend = audioItem.spatialBlend;
        }

        private void OnGetFromPool(AudioSource audioSource)
        {
            audioSource.enabled = true;
        }

        private void OnReleaseToPool(AudioSource audioSource)
        {
            audioSource.enabled = false;
        }

        private void OnDestroyPooled(AudioSource audioSource)
        {
            Destroy(audioSource);
        }
    }
}