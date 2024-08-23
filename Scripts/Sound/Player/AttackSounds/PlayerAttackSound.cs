using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class PlayerAttackSound : MonoBehaviour
    {
        public AudioClip[] clipsMiss;
        public AudioSource source;

        public void PlayRandomAttackClip()
        {
            source.clip = clipsMiss[Random.Range(0, clipsMiss.Length)];
            source.Play();
        }
    }
}