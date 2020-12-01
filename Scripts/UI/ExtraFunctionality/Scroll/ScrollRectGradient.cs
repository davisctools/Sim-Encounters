using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class ScrollRectGradient : MonoBehaviour
    {
        public Image BottomGradient { get => bottomGradient; set => bottomGradient = value; }
        [SerializeField] private Image bottomGradient;
        public Image TopGradient { get => topGradient; set => topGradient = value; }
        [SerializeField] private Image topGradient;

        private ScrollRect scrollRect;
        protected ScrollRect ScrollRect {
            get {
                if (scrollRect == null)
                    scrollRect = GetComponent<ScrollRect>();
                return scrollRect;
            }
        }

        protected virtual void Start() => ResetGradients();

        private float currentTopTransparency, currentBottomTransparency;

        private int resetFrames;
        public virtual void ResetGradients()
        {
            resetFrames = 2;
            ResetBottomGradient();
            ResetTopGradient();
        }

        protected virtual void Update()
        {
            if (!ScrollRect.verticalScrollbar.gameObject.activeSelf) {
                HideBottomGradient();
                HideTopGradient();
                return;
            }

            if (resetFrames <= 0) {
                UpdateBottomGradient();
                UpdateTopGradient();
                return;
            }
            resetFrames--;
            ResetBottomGradient();
            ResetTopGradient();
        }

        protected virtual void HideBottomGradient()
        {
            if (BottomGradient == null)
                return;
            currentBottomTransparency = 0;
            UpdateBottomGradientColor();
        }
        protected virtual void HideTopGradient()
        {
            if (TopGradient == null)
                return;
            currentTopTransparency = 0;
            UpdateTopGradientColor();
        }

        protected virtual void ResetBottomGradient()
        {
            if (BottomGradient == null)
                return;
            currentBottomTransparency = GetPreferredBottomTransparency();
            UpdateBottomGradientColor();
        }

        protected virtual void ResetTopGradient()
        {
            if (TopGradient == null)
                return;
            currentTopTransparency = GetTopPreferredTransparency();
            UpdateTopGradientColor();
        }

        protected virtual void UpdateBottomGradient()
        {
            if (BottomGradient == null)
                return;
            currentBottomTransparency = GetNewCurrent(currentBottomTransparency, GetPreferredBottomTransparency());
            UpdateBottomGradientColor();
        }

        protected virtual void UpdateTopGradient()
        {
            if (TopGradient == null)
                return;
            currentTopTransparency = GetNewCurrent(currentTopTransparency, GetTopPreferredTransparency());
            UpdateTopGradientColor();
        }

        protected virtual void UpdateBottomGradientColor()
            => UpdateGradientColor(bottomGradient, currentBottomTransparency);
        protected virtual void UpdateTopGradientColor()
            => UpdateGradientColor(topGradient, currentTopTransparency);
        protected virtual void UpdateGradientColor(Image gradient, float transparency)
            => gradient.color = new Color(1, 1, 1, transparency);

        protected virtual float GetPreferredBottomTransparency()
            => GetPreferred(ScrollRect.verticalNormalizedPosition);
        protected virtual float GetTopPreferredTransparency()
            => GetPreferred(1 - ScrollRect.verticalNormalizedPosition);
        private const float ScrollAmount = .15f;
        protected virtual float GetPreferred(float distFromEnd)
        {
            if (ScrollRect.content.rect.height <= ScrollRect.viewport.rect.height)
                return 0;
            else if (distFromEnd >= ScrollAmount)
                return 1;
            else
                return distFromEnd / ScrollAmount;
        }

        private const float FadeTime = .2f;
        protected virtual float GetNewCurrent(float current, float preferred)
        {
            if (current > preferred) {
                current -= Time.deltaTime / FadeTime;
                return Mathf.Max(current, preferred);
            } else if (current < preferred) {
                current += Time.deltaTime / FadeTime;
                return Mathf.Min(current, preferred);
            } else {
                return current;
            }
        }
    }
}
