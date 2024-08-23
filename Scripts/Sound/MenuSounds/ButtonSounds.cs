using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void HoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void ClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
