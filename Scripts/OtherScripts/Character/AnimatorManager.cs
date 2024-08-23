using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void PlayAnimation(string animationName, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(animationName, 0.2f);
        }

    }
}