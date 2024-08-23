using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF {     
    public class DamageColider : MonoBehaviour
    {
        public Collider damageCollider;
        public Collider damageCollider2;

        private void Awake()
        {
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
            if (damageCollider2 != null)
            {
                damageCollider2.gameObject.SetActive(true);
                damageCollider2.isTrigger = true;
                damageCollider2.enabled = false;
            }
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider2()
        {
            damageCollider2.enabled = true;
        }

        public void DisableDamageCollider2()
        {
            damageCollider2.enabled = false;
        }

        public void DisableAllDamageColliders()
        {
            damageCollider.enabled = false;
            if (damageCollider2 != null)
            {
                damageCollider2.enabled = false;
            }
        }
    }
}