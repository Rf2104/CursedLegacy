using System.Collections;
using System.Collections.Generic;
using RF;
using UnityEngine;

public class CollisionDetectorEnemy : MonoBehaviour
{
    [SerializeField]
    public int EnemyDamage = 10;

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerStats>().TakeDamage(EnemyDamage);
        }
    }
}
