using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class Billboard : MonoBehaviour
    {
        private Transform cam;

        private void Awake()
        {
            cam = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}