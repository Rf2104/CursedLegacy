using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RF {
    public class WalkingSounds : MonoBehaviour
    {
        public AudioClip[] clips;
        public AudioSource source;
        public PlayerController playerController;

        public void PlayRandomClip(float TargetWalkSpeed)
        {
            float actualSpeed = playerController.GetMovSpeed();

            if (GetMovState(TargetWalkSpeed) == GetMovState(actualSpeed))
            {
                source.clip = clips[Random.Range(0, clips.Length)];
                source.Play();
            }
        }

        public int GetMovState(float speed)
        {
            if (speed < 4.1f)
            {
                return 1;
            }

            return 2;

        }
    }
}