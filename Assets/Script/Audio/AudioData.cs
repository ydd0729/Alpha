using UnityEngine;
using UnityEngine.Audio;
using Yd.Collection;

namespace Yd.Audio
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/Audio/AudioData")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private SDictionary<AudioId, Object> audios;

        [SerializeField] private SDictionary<AudioChannel, AudioMixerGroup> audioMixerGroups;

        public SDictionary<AudioId, Object> AudioResources => audios;

        public SDictionary<AudioChannel, AudioMixerGroup> AudioMixerGroups => audioMixerGroups;
    }
}