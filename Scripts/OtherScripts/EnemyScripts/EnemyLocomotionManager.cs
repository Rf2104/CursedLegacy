using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace RF
{
    public class EnemyLocomotionManager : MonoBehaviour
    {

        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;
        NavMeshAgent navMeshAgent;
        public Rigidbody enemyRB;

        public PlayerStats currentTarget;
        public LayerMask detectionLayer;

        public float stoppingDistance = 2f;
        public float distanceFromTarget;

        public float rotationSpeed = 10f;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRB = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRB.isKinematic = false;
        }

        public bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
        }

        public void HandleDetection()
        {
            enemyAnimatorManager.anim.SetFloat("Speed", 0, 0.4f, Time.deltaTime); //moveSpeed = 0
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer); // colliders in the detection radius of the enemy

            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

                if (playerStats != null) // if the collider has a characterStats component (is a character))
                {
                    Vector3 targetPosition = playerStats.transform.position;
                    Vector3 direction = targetPosition - transform.position;
                    float viewableAngle = Vector3.Angle(direction, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        currentTarget = playerStats;
                    }
                }
            }
        }

        public void HandleMoveToTarget()
        {

            Vector3 targetDirection = (currentTarget.transform.position - transform.position).normalized;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            // if preforming action, stop moving
            if(enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime); //moveSpeed = 0
                navMeshAgent.enabled = true;
            }
            else 
            { 
                if(distanceFromTarget > stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Speed", 1, 0.2f, Time.deltaTime); //moveSpeed = 1
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime); //moveSpeed = 0
                }
            }

            HandleRotateTowardsTarget();
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        public void HandleRotateTowardsTarget()
        {
            // rotate manually
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.5f * Time.fixedDeltaTime);
                Debug.Log("Rotating manually");
            }
            else // rotate using Navmesh
            {
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentTarget.transform.position);
                enemyRB.velocity = navMeshAgent.speed * transform.forward;
                transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }

        public void TargetPlayer() //if the Player Attacks the Enemy from behind, the Enemy will target the Player
        {
            enemyAnimatorManager.anim.SetFloat("Speed", 0, 0.4f, Time.deltaTime); //moveSpeed = 0
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer); // colliders in the detection radius of the enemy

            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

                if (playerStats != null) // if the collider has a characterStats component (is a character))
                {
                    currentTarget = playerStats;
                }
            }
        }

        public void HandleCloseRotations()
        {
            navMeshAgent.enabled = false;
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            targetDirection.Normalize();
            targetDirection.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), 3.5f * Time.deltaTime);

        }
    }
}