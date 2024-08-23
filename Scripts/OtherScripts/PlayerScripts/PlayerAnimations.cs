using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class PlayerAnimations : AnimatorManager
    {
        public PlayerController playerController;

        void Start()
        {
            playerController = GetComponent<PlayerController>();
        }

        public void SetMovementAnimation()
        {
            // Set the animation based on the player's movement speed
            float moveSpeed = playerController.moveDir.magnitude;
            if (moveSpeed <= 0)
            {
                anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
            }
            else if (moveSpeed > 0 && moveSpeed < 3)
            {
                anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
            }
            else if (moveSpeed >= 3 && moveSpeed < 5.1)
            {
                anim.SetFloat("Speed", 2, 0.1f, Time.deltaTime);
            }
            else if (playerController.sprintFlag)
            {
                anim.SetFloat("Speed", 3, 0.1f, Time.deltaTime);
            }
        }

        public void EnableCombo()
        {
            anim.SetBool("canCombo", true);
        }
        public void DisableCombo()
        {
            anim.SetBool("canCombo", false);
        }

        public void CheckCombo()
        {
            anim.SetBool("checkCombo", true);
        }

        public void ResetCheckCombo()
        {
            anim.SetBool("checkCombo", false);
        }

        public void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0; 
            Vector3 velocity = (deltaPosition / delta) * 1.35f;
            playerController.controller.Move(velocity * delta);

            if (!playerController.controller.isGrounded)
                playerController.controller.Move(Vector3.down * playerController.gravity * Time.deltaTime * 1.5f);
        }

    }
}
