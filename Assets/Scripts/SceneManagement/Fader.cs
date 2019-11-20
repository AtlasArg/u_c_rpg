using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        
        public IEnumerator FadeOut(float time)
        {
            if (this.canvasGroup == null)
            {
                this.canvasGroup = this.GetComponent<CanvasGroup>();
            }

            while (this.canvasGroup.alpha < 1)
            {
                this.canvasGroup.alpha += Time.deltaTime / time;
                yield return null;  
            }
        }

        public IEnumerator FadeIn(float time)
        {
            if (this.canvasGroup == null)
            {
                this.canvasGroup = this.GetComponent<CanvasGroup>();
            }

            while (this.canvasGroup.alpha > 0)
            {
                this.canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }

        public void FadeOutInmediate()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1;
            }
        }
    
        private void Start()
        {
            this.canvasGroup = this.GetComponent<CanvasGroup>();
        }
    }
}
