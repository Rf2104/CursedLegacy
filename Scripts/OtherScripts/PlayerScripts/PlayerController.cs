using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace RF
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject playerPrefab;

        public Cinemachine.CinemachineFreeLook freeLookCamera;

        [Header("Variables")]
        public float movSpeed = 4f;
        public float sprintSpeed = 5.5f;
        public float rotSpeed = 10f;
        //float rollSpeed = 20f;
        public float gravity = 10f;

        public CharacterController controller;
        PlayerAnimations playerAnimations;
        PlayerStats playerStats;
        WeaponFX weaponFX;
        public Vector3 moveDir = Vector3.zero;

        public bool canMove = true;
        public bool rollFlag = false;
        public bool sprintFlag = false;
        public float rollTimer;

        private bool pressedCombo = false;

        public bool imunityFrames = false;

        private Coroutine staminaRestoreCoroutine;

        private Vector2 input;
        private Quaternion freeRotation;
        private Vector3 targetDirection;

        void Start()
        {
            playerStats = GetComponent<PlayerStats>();
            freeLookCamera = FindObjectOfType<Cinemachine.CinemachineFreeLook>();
            controller = GetComponent<CharacterController>();
            playerAnimations = GetComponent<PlayerAnimations>();
            weaponFX = GetComponentInChildren<WeaponFX>();
            weaponFX.weaponTrail.Stop();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {

            if (freeLookCamera.LookAt == null || freeLookCamera.Follow)
            {
                freeLookCamera.Follow = GameObject.FindGameObjectWithTag("Follow").transform;
                freeLookCamera.LookAt = GameObject.FindGameObjectWithTag("Look").transform;
            }

            // Handle stamina restore
            if (!sprintFlag && !rollFlag && !playerAnimations.anim.GetBool("isAttacking") && staminaRestoreCoroutine == null)
            {
                staminaRestoreCoroutine = StartCoroutine(RestoreStaminaOverTime());
            }
            else if (sprintFlag || rollFlag || playerAnimations.anim.GetBool("isAttacking"))
            {
                if (staminaRestoreCoroutine != null)
                {
                    StopCoroutine(staminaRestoreCoroutine);
                    staminaRestoreCoroutine = null;
                }
            }

            // Handle attack
            HandleAttack();

            HandleMovement();

            // Handle roll and backstep
            HandleRollAndBackstep();

            // Update the animator
            playerAnimations.SetMovementAnimation();
        }

        private void LateUpdate()
        {
            rollFlag = false;
        }

        // Handle player movement
        void HandleMovement()
        {
            if (playerAnimations.anim.GetBool("isInteracting"))
                return;

            if (!playerAnimations.anim.GetBool("comboTerminou"))
                return;

            // Read input
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // if is sprinting, set speed to sprintSpeed, else set speed to movSpeed
            float curSpeed = canMove ? (sprintFlag ? sprintSpeed : movSpeed) : 0;

            // Mathf.Abs to avoid negative input values
            float move = Mathf.Abs(input.y) + Mathf.Abs(input.x);

            // get the forward direction of the player
            Vector3 forward = transform.forward;
            moveDir = forward * move;
            moveDir.Normalize(); // normalize vector to avoid diagonal speed boost
            moveDir *= curSpeed;

            IsSprinting();
            UpdateTargetDirection();

            // Rotate player towards movement direction
            if (input != Vector2.zero && targetDirection.magnitude > 0.1f) // only rotates player if there is input
            {
                // get normalized direction obtained from UpdateTargetDirection()
                Vector3 lookDirection = targetDirection.normalized;

                // represents the rotation that the player has to do
                freeRotation = Quaternion.LookRotation(lookDirection, transform.up);

                // smoothly rotates the player using Slerp
                transform.rotation = Quaternion.Slerp(transform.rotation, freeRotation, rotSpeed * Time.deltaTime);
            }

            // Apply gravity
            if (!controller.isGrounded)
            {
                controller.Move(Vector3.down * gravity * Time.deltaTime * 1.5f);
            }

            // Move the controller
            if (input != Vector2.zero)
            {
                controller.Move(moveDir * Time.deltaTime);
            }
        }

        // Update the target direction of the player based on the camera's forward and right vectors
        public void UpdateTargetDirection()
        {
            var forward = Camera.main.transform.forward;
            forward.y = 0f;

            var right = Camera.main.transform.right;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            targetDirection = input.x * right + input.y * forward;
        }

        // Check if the player is sprinting or rolling
        public void IsSprinting()
        {
            if (playerStats.currentStamina > 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton1))
                {
                    rollTimer += Time.deltaTime;
                    sprintFlag = true;
                    playerStats.UseStamina(1);
                }
                else
                {
                    if (rollTimer > 0 && rollTimer < 0.2f) // if the player presses the sprint button for less than 0.2 seconds, roll
                    {
                        sprintFlag = false;
                        rollFlag = true;
                        playerStats.UseStamina(150);
                    }
                    rollTimer = 0;
                }
                // if the player releases the sprint button, stop sprinting
                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.JoystickButton1))
                {
                    sprintFlag = false;
                }
            }
            else
            {
                sprintFlag = false;
            }
        }

        public void HandleRollAndBackstep()
        {
            if (rollFlag)
            {
                if (moveDir.magnitude <= 0) // if the player is not moving, play the backstep animation
                {
                    playerAnimations.PlayAnimation("Backstep", true);
                }
                if (moveDir.magnitude > 0) // if the player is moving, play the roll animation
                {
                    playerAnimations.PlayAnimation("Roll", true);
                    // start a coroutine to activate the imunity frames
                    imunityFrames = true;
                    StartCoroutine(StartImunityFrames());
                }
            }
            return;
        }

        IEnumerator StartImunityFrames()
        {
            yield return new WaitForSeconds(1);
            imunityFrames = false;
        }

        public void HandleAttack()
        {
            if (playerStats.currentStamina > 0)
            {
                if (playerAnimations.anim.GetBool("isAttacking"))
                {
                    // check if can combo according to the animation
                    if (playerAnimations.anim.GetBool("canCombo"))
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                        {
                            pressedCombo = true;
                            playerAnimations.anim.SetBool("isAttacking", true);
                        }

                        // when the first attack animation reaches the checkCombo and if the player pressed the attack button, play the second attack animation
                        //this first if is to prevent from the second attack animation to be played before the first attack animation ends
                        if (playerAnimations.anim.GetBool("checkCombo"))
                        {
                            if (pressedCombo)
                            {
                                playerAnimations.DisableCombo();
                                pressedCombo = false;
                                playerAnimations.PlayAnimation("LightAttack2", true);
                                playerAnimations.anim.SetBool("comboTerminou", false);
                                playerAnimations.anim.SetFloat("Speed", 0);
                                playerStats.UseStamina(150);
                            }
                        }
                    }
                }
                else if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Joystick1Button5)) && playerAnimations.anim.GetBool("comboTerminou")
                    && !playerAnimations.anim.GetBool("isAttacking") && !playerAnimations.anim.GetBool("isInteracting"))
                {
                    playerAnimations.anim.SetFloat("Speed", 0, 0.5f, Time.deltaTime);
                    playerAnimations.PlayAnimation("LightAttack1", true);
                    weaponFX.PlayWeaponFX();
                    playerAnimations.anim.SetBool("isAttacking", true);
                    playerStats.UseStamina(150);
                }
                else
                    return;
            }
        }

        //restore stamina over time after 1 second of not using stamina
        IEnumerator RestoreStaminaOverTime()
        {
            yield return new WaitForSeconds(2);
            while (playerStats.currentStamina < playerStats.maxStamina)
            {
                playerStats.currentStamina += playerStats.maxStamina / 100;
                playerStats.staminaBar.SetCurrentStamina(playerStats.currentStamina);
                yield return new WaitForSeconds(0.01f);
            }
            staminaRestoreCoroutine = null;
        }

        public float GetMovSpeed()
        {
            float speed = controller.velocity.magnitude;

            return speed;
        }

        public void RespawnPlayer()
        {
            FindObjectOfType<EnemySpawn>().SpawnEnemies(playerStats.bossesKilled);
            int checkpoint = playerStats.checkPointID;
            Transform currentCheckpoint;

            if (checkpoint == 0)
                currentCheckpoint = GameObject.FindGameObjectWithTag("Start").transform;
            else if (checkpoint == 1)
                currentCheckpoint = GameObject.FindGameObjectWithTag("Mid").transform;
            else
                currentCheckpoint = GameObject.FindGameObjectWithTag("End").transform;

            Instantiate(playerPrefab, currentCheckpoint.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}