using System.Collections;
using System.Collections.Generic;
using RF;
using UnityEngine;

namespace RF
{
    [CreateAssetMenu(menuName = "Enemy Actions")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3; // the score of the attack action to determine the priority of the attack

        public float recoveryTime = 2f;

        public float maximumAttackAngle = 50f;
        public float minimumAttackAngle = -50f;

        public float minimumDistanceNeededToAttack = 0f;
        public float maximumDistanceNeededToAttack = 1.1f;
    }
}