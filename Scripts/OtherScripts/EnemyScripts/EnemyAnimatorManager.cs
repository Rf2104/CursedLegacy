using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyLocomotionManager enemyLocomotionManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
        }

        private void OnAnimatorMove()
        {
            // if timescale is 0, the game is paused
            if (Time.timeScale < 1)
            {
                return;
            }

            float delta = Time.deltaTime;
            if (delta > 0)
            {
                enemyLocomotionManager.enemyRB.drag = 0;
                Vector3 deltaPosition = anim.deltaPosition;
                deltaPosition.y = 0;
                Vector3 velocity = deltaPosition / delta;
                enemyLocomotionManager.enemyRB.velocity = velocity;
            }
        }
    }
}