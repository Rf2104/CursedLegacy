using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF {
    public class EnemyManager : MonoBehaviour
    {
        private EnemyLocomotionManager enemyLocomotionManager;
        public bool isPerformingAction;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyAttackAction currentAttack;
        public EnemyStats enemystats;

        [Header("AI Settings")]
        public float detectionRadius;
        public float minimumDetectionAngle;
        public float maximumDetectionAngle;
        public float attackDistance;

        public float currentRecoveryTime = 0;

        public float viewableAngle;
        Vector3 targetDirection;

        void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemystats = GetComponent<EnemyStats>();
        }

        void Update()
        {
            HandleRecoveryTimer();
            // apply gravity to the enemy

            if (!enemyLocomotionManager.IsGrounded())
            {
                enemyLocomotionManager.enemyRB.AddForce(Vector3.down * 100);
            }
        }

        private void FixedUpdate() // Update is called once per frame
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemystats.isDead)
            {
                return;
            }

            // if the enemy has a target, check the distance and angle to the player
            if (enemyLocomotionManager.currentTarget != null)
            {
                enemyLocomotionManager.distanceFromTarget = Vector3.Distance(transform.position, enemyLocomotionManager.currentTarget.transform.position);
                targetDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            }

            // if the enemy doesnt have a target, search for the player
            if (enemyLocomotionManager.currentTarget == null) 
            { 
                enemyLocomotionManager.HandleDetection();
            }
            // if the player is too far, search for the player
            else if (enemyLocomotionManager.distanceFromTarget > detectionRadius)
            {
                enemyLocomotionManager.currentTarget = null;
                enemyLocomotionManager.HandleDetection();
            }
            // if the player is too far, move to the player
            else if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
            {
                enemyLocomotionManager.enemyRB.constraints = RigidbodyConstraints.FreezeRotation;
                enemyLocomotionManager.HandleMoveToTarget();
            }
            // if the player is close and in front, attack the player
            else if(enemyLocomotionManager.distanceFromTarget <= attackDistance && viewableAngle <= maximumDetectionAngle && viewableAngle >= minimumDetectionAngle)
            {
                enemyLocomotionManager.enemyRB.constraints = RigidbodyConstraints.FreezeAll;
                AttackTarget();
            }
            // if the enemy is close but backwards, rotates to face the player
            else if (enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance && !isPerformingAction)
            {
                enemyLocomotionManager.enemyRB.constraints = RigidbodyConstraints.FreezeRotation;
                enemyLocomotionManager.HandleCloseRotations();
            }
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        #region Attacks

        // attack the player
        private void AttackTarget()
        {
            if (isPerformingAction)
            {
                return;
            }

            if (currentAttack == null)
            {
                GetNewAttack();
            }
            else
            {
                isPerformingAction = true;
                currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorManager.PlayAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }
        }

        // get a new attack based on the distance and angle to the player
        private void GetNewAttack()
        {
            targetDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
            viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            enemyLocomotionManager.distanceFromTarget = Vector3.Distance(transform.position, enemyLocomotionManager.currentTarget.transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                // if the player is in the attack range and angle, add the score of the attack to the max score
                if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore; // add the score of the attack to the max score 
                    }
                }
            }

            // get a random value between 0 and the max score
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            // get the attack with the highest score
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                        {
                            return;
                        }

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        #endregion
    }

}