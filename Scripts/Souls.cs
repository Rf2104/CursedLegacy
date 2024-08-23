using System.Collections;
using System.Collections.Generic;
using RF;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{
    public class Souls : MonoBehaviour
    {
        public TextMeshProUGUI soulsText;

        public void UpdateUIText(int souls)
        {
            soulsText.text = "" + souls;
        }
    }
}
