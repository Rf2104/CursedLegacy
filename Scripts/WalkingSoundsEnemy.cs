using System.Collections;
using System.Collections.Generic;
using RF;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class WalkingSoundsEnemy : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource source;
    public EnemyLocomotionManager enemyLocomotionManager;

    public void Awake()
    {
        enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
    }
    public void PlayRandomEnemyWalkingClip()
    {
        if (enemyLocomotionManager.currentTarget != null)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();
        }
    }
}
