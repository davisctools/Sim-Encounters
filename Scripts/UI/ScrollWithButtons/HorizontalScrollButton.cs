using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    public class HorizontalScrollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private CanvasGroup canvasGroup;

        public bool IsDown { get; set; } = false;

        public virtual void OnPointerDown(PointerEventData eventData) => IsDown = true;
        public virtual void OnPointerUp(PointerEventData eventData) => IsDown = false;

        protected Coroutine CurrentCoroutine { get; set; }
        public virtual void SetActive(bool value)
        {
            if (!value)
                IsDown = false;

            if (CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);

            if (value)
                gameObject.SetActive(true);

            if (gameObject.activeInHierarchy)
                CurrentCoroutine = StartCoroutine(value ? Show() : Hide());
        }

        private const float AnimationTime = .15f;
        public virtual IEnumerator Show()
        {
            while (canvasGroup.alpha < 1) {
                canvasGroup.alpha += Time.deltaTime / AnimationTime;
                yield return null;
            }
        }

        public virtual IEnumerator Hide()
        {
            while (canvasGroup.alpha > 0) {
                canvasGroup.alpha -= Time.deltaTime / AnimationTime;
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}