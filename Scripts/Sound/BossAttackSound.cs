using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class BossAttackSound : MonoBehaviour
    {
        public AudioClip[] Attackclips;
        public AudioClip IdleClip;
        public AudioClip DeathClips;
        private AudioSource source;

        public void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayRandomAttackClip()
        {
            // 85% chance to play the clip
            if (Random.Range(0, 100) > 85)
            {
                return;
            }
            source.clip = Attackclips[Random.Range(0, Attackclips.Length)];
            source.Play();
        }

        public void PlayIdleClip()
        {
            // if any clip is playing, return
            if (source.isPlaying)
            {
                return;
            }
            source.clip = IdleClip;
            source.Play();
        }

        public void PlayDeathClip()
        {
            source.clip = DeathClips;
            source.Play();
        }
    }
}