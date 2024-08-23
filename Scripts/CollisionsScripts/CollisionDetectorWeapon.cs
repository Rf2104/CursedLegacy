using System.Collections;
using System.Collections.Generic;
using RF;
using UnityEngine;

public class CollisionDetectorWeapon : MonoBehaviour
{
    public AudioClip[] clipsHit;
    private AudioSource source;
    private PlayerStats playerStats;

    public void Awake()
    {
        source = GetComponent<AudioSource>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            PlayRandomHitClip();
            other.GetComponent<EnemyStats>().TakeDamageEnemy(playerStats.damage);
        }

        if (other.tag == "Boss1")
        {
            PlayRandomHitClip();
            other.GetComponent<EnemyStats>().TakeDamageBoss(playerStats.damage);
        }

        if (other.tag == "Boss2")
        {
            PlayRandomHitClip();
            other.GetComponent<EnemyStats>().TakeDamageBoss(playerStats.damage);
        }
    }

    public void PlayRandomHitClip()
    {
        source.clip = clipsHit[Random.Range(0, clipsHit.Length)];
        source.Play();
    }
}
