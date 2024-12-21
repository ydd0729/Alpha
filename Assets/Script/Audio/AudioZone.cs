using System;
using System.Collections.Generic;
using UnityEngine;
using Yd.PhysicsExtension;

namespace Yd.Audio
{
    public class AudioZone : MonoBehaviour
    {
        [SerializeField] private float innerRadius;
        [SerializeField] private float outerRadius;
        [SerializeField] private AudioId[] audios;
        [SerializeField] private AudioChannel channel;
        
        private AudioManager audioManager;

        private void Awake()
        {
            audioManager = GetComponent<AudioManager>();
        }

        private void Start()
        {
            if (audios.Length == 0)
            {
                return;
            }
            
            foreach (var audioId in audios)
            {
                var audioSource = audioManager.PlayLoop(audioId, channel);
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                
                if (outerRadius == 0)
                {
                    audioSource.spatialize = false;
                }
                else
                {
                    audioSource.spatialize = true;
                    audioSource.minDistance = innerRadius;
                    audioSource.maxDistance = outerRadius;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gray;
            
            Gizmos.DrawWireSphere(transform.position, innerRadius);
            Gizmos.DrawWireSphere(transform.position, outerRadius);
        }
    }
}