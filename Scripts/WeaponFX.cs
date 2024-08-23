using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("Weapon")]
        public ParticleSystem weaponTrail;

        public void PlayWeaponFX()
        {
            weaponTrail.Stop();

            if (weaponTrail.isStopped)
            {
                weaponTrail.Play();
            }
        }

    }
}