using System.Collections;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImediatelly()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                Debug.Log("FADE OUT");
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                Debug.Log("FADE IN");
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}