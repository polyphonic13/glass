namespace Polyworks
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class NotificationUI : MonoBehaviour
    {
        private float fadeTime;
        private float fadeSpeed;

        private Text textField;
        private CanvasGroup canvasGroup;
        private float startAlpha;

        private bool isDisplayingNote = false;

        public bool IsDisplayingNote
        {
            get
            {
                return isDisplayingNote;
            }
        }

        public void RemoveNote()
        {
            Hide();
        }

        public void AddNote(string message)
        {
            isDisplayingNote = true;
            canvasGroup.alpha = startAlpha;
            textField.text = message;
            StartCoroutine("fade");
        }

        public void Init(float fadeTime, float fadeSpeed)
        {
            this.fadeTime = fadeTime;
            this.fadeSpeed = fadeSpeed;
            textField = GetComponentInChildren<Text>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();

            startAlpha = canvasGroup.alpha;
            canvasGroup.alpha = 0;
        }

        public void Hide()
        {
            isDisplayingNote = false;
            textField.text = "";
            canvasGroup.alpha = 0;
        }

        private IEnumerator fade()
        {
            for (float f = fadeTime; f >= 0; f -= fadeSpeed)
            {
                if (f <= 1f)
                {
                    if (f < 0.1f)
                    {
                        f = 0;
                    }
                    canvasGroup.alpha = f;
                    if (f == 0)
                    {
                        RemoveNote();
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}

