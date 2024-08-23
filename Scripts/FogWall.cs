using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class FogWall : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void AtivateFogWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFogWall()
        {
            gameObject.SetActive(false);
        }
    }
}