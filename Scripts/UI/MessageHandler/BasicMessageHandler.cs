using System.Collections;
using TMPro;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class BasicMessageHandler : BaseBasicMessageHandler
    {
        public CanvasGroup Group { get => group; set => group = value; }
        [SerializeField] private CanvasGroup group;
        public TextMeshProUGUI MessageLabel { get => messageLabel; set => messageLabel = value; }
        [SerializeField] private TextMeshProUGUI messageLabel;
        
        private IEnumerator currentFadeEnumerator;

        public override void ShowMessage(string message)
        {
            if (currentFadeEnumerator != null)
                StopCoroutine(currentFadeEnumerator);

            Group.alpha = 1;
            gameObject.SetActive(true);
            MessageLabel.text = message;

            currentFadeEnumerator = Fade();
            StartCoroutine(currentFadeEnumerator);
        }


        private const float FADE_DELAY = 5;
        private const float FADE_TIME = 3;
        private IEnumerator Fade()
        {
            yield return new WaitForSeconds(FADE_DELAY);

            while (Group.alpha > 0) {
                Group.alpha -= Time.deltaTime / FADE_TIME;
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}