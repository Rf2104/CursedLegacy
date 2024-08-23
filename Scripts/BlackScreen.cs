using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{
    public class BlackScreen : MonoBehaviour
    {
        public Image blackScreen;

        public void HandleBlackScreen() 
        {
            StartCoroutine(HandleDeathAndRespawn());
        }

        public IEnumerator HandleDeathAndRespawn()
        {
            yield return new WaitForSeconds(0.2f); // Ensure animation starts
            yield return FadeToBlack(1.0f, 2.0f); // Fade to black over 3 seconds
            FindObjectOfType<PlayerController>().enabled = true;
            FindObjectOfType<EnemySpawn>().RemoveAllEnemies();
            yield return new WaitForSeconds(1f); // Ensure Enemies are removed
            FindObjectOfType<PlayerController>().RespawnPlayer();
            yield return new WaitForSeconds(1f); // Ensure camera is in position
            yield return FadeFromBlack(0.0f, 2.0f); // Fade back to game
        }

        private IEnumerator FadeToBlack(float targetAlpha, float duration)
        {
            float startAlpha = blackScreen.color.a;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
                yield return null;
            }

            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, targetAlpha);
        }

        private IEnumerator FadeFromBlack(float targetAlpha, float duration)
        {
            float startAlpha = blackScreen.color.a;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
                yield return null;
            }

            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, targetAlpha);
        }
    }

}