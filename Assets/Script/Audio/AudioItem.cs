using System;
using UnityEngine;

namespace Yd.Audio
{
    [Serializable]
    public struct AudioItem
    {
        public AudioClip audioClip;
        public float volume;
        public float pitch;
        [NonSerialized] public float spatialBlend;
    }
}