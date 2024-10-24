using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Yd.Algorithm;
using Yd.Collection;
using Random = UnityEngine.Random;

namespace Yd.Audio
{
    [CreateAssetMenu(fileName = "Audio Container", menuName = "Scriptable Objects/Audio/Audio Container")]
    public class AudioContainer : ScriptableObject, IEnumerable<AudioItem>
    {
        [Serializable]
        public enum AudioContainerMode
        {
            Sequential,
            Shuffle,
            Random
        }

        [SerializeField] private float volume = 1;
        [SerializeField] private SRange<float> volumeRandomRange;
        [SerializeField] private float pitch = 1;
        [SerializeField] private SRange<float> pitchRandomRange;
        [SerializeField] private float spatialBlend = 1;
        [SerializeField] private AudioContainerMode playbackMode;
        [SerializeField] private int avoidRepeatingLast;
        [SerializeField] private List<AudioItem> audioClips;

        private AudioItem this[int index]
        {
            get
            {
                var audioItem = audioClips[index];

                audioItem.volume += volume + Random.Range(volumeRandomRange.Min, volumeRandomRange.Max);
                audioItem.volume = Mathf.Clamp(audioItem.volume, 0, 1);
                audioItem.pitch += pitch + Random.Range(pitchRandomRange.Min, pitchRandomRange.Max);
                audioItem.spatialBlend = spatialBlend;

                return audioItem;
            }
        }

        private void OnValidate()
        {
            volume = Mathf.Clamp(volume, 0, 1);
            spatialBlend = Mathf.Clamp(spatialBlend, 0, 1);
            volumeRandomRange.Min = Mathf.Clamp(volumeRandomRange.Min, -volume, 1 - volume);
            volumeRandomRange.Max = Mathf.Clamp(volumeRandomRange.Max, volumeRandomRange.Min, 1 - volume);
        }

        public IEnumerator<AudioItem> GetEnumerator()
        {
            return playbackMode switch
            {
                AudioContainerMode.Sequential => audioClips.GetEnumerator(),
                AudioContainerMode.Shuffle => new ShuffleEnumerator(this),
                AudioContainerMode.Random => new RandomEnumerator(this),
                _ => throw new InvalidEnumArgumentException($"PlaybackMode = {(int)playbackMode}")
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ShuffleEnumerator : IEnumerator<AudioItem>
        {
            private readonly AudioContainer audioContainer;
            private List<int> shuffledIndex;
            private IEnumerator<int> shuffledIndexEnumerator;

            public ShuffleEnumerator(AudioContainer audioContainer)
            {
                this.audioContainer = audioContainer;

                Reset();
            }

            public AudioItem Current => audioContainer[shuffledIndexEnumerator.Current];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                shuffledIndexEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                return shuffledIndexEnumerator.MoveNext();
            }

            public void Reset()
            {
                shuffledIndex = new List<int>(audioContainer.audioClips.Count);
                for (var i = 0; i < audioContainer.audioClips.Count; i++)
                {
                    shuffledIndex.Add(i);
                }

                shuffledIndex.Shuffle();
                shuffledIndexEnumerator = shuffledIndex.GetEnumerator();
            }
        }

        private class RandomEnumerator : IEnumerator<AudioItem>
        {
            private readonly AudioContainer audioContainer;
            private readonly LinkedList<int> canPlay;
            private readonly Queue<int> played;
            private int currentIndex;

            public RandomEnumerator(AudioContainer audioContainer)
            {
                this.audioContainer = audioContainer;
                played = new Queue<int>();
                canPlay = new LinkedList<int>();

                Reset();
            }

            public AudioItem Current => audioContainer[currentIndex];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                currentIndex = canPlay.ElementAt(Random.Range(0, canPlay.Count));

                if (played.Count == audioContainer.avoidRepeatingLast)
                {
                    canPlay.AddLast(played.Dequeue());
                }

                if (played.Count < audioContainer.avoidRepeatingLast)
                {
                    played.Enqueue(currentIndex);
                    canPlay.Remove(currentIndex);
                }

                return true;
            }

            public void Reset()
            {
                canPlay.Clear();
                played.Clear();
                for (var i = 0; i < audioContainer.audioClips.Count; i++)
                {
                    canPlay.AddLast(i);
                }
            }
        }
    }
}