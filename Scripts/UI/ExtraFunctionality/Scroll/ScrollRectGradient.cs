using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectGradient : MonoBehaviour
    {
        public Image BottomGradient { get => bottomGradient; set => bottomGradient = value; }
        [SerializeField] private Image bottomGradient;
        public Image TopGradient { get => topGradient; set => topGradient = value; }
        [SerializeField] private Image topGradient;
        public Image LeftGradient { get => leftGradient; set => leftGradient = value; }
        [SerializeField] private Image leftGradient;
        public Image RightGradient { get => rightGradient; set => rightGradient = value; }
        [SerializeField] private Image rightGradient;

        private ScrollRect scrollRect;
        protected ScrollRect ScrollRect => scrollRect != null ? scrollRect : (scrollRect = GetComponent<ScrollRect>());

        protected virtual void Start() => ResetGradients();

        private float currentTopTransparency, currentBottomTransparency, currentLeftTransparency, currentRightTransparency;

        private int resetFrames;
        public virtual void ResetGradients()
        {
            resetFrames = 2;
            ResetGradientsToPreferred();
        }

        protected virtual void Update()
        {
            if (!ScrollRect.verticalScrollbar.gameObject.activeSelf) {
                HideGradients();
            } else if (resetFrames <= 0) {
                UpdateGradients();
            } else {
                resetFrames--;
                ResetGradientsToPreferred();
            }
        }


        protected virtual void HideGradients()
        {
            HideBottomGradient();
            HideTopGradient();
            HideLeftGradient();
            HideRightGradient();
        }
        protected virtual void HideBottomGradient()
            => UpdateGradient(BottomGradient, UpdateBottomGradientColor, 0);
        protected virtual void HideTopGradient()
            => UpdateGradient(TopGradient, UpdateTopGradientColor, 0);
        protected virtual void HideLeftGradient()
            => UpdateGradient(LeftGradient, UpdateLeftGradientColor, 0);
        protected virtual void HideRightGradient()
            => UpdateGradient(RightGradient, UpdateRightGradientColor, 0);

        protected virtual void ResetGradientsToPreferred()
        {
            ResetBottomGradient();
            ResetTopGradient();
            ResetLeftGradient();
            ResetRightGradient();
        }
        protected virtual void ResetBottomGradient()
            => UpdateGradient(BottomGradient, UpdateBottomGradientColor, GetPreferredBottomTransparency());
        protected virtual void ResetTopGradient()
            => UpdateGradient(TopGradient, UpdateTopGradientColor, GetPreferredTopTransparency());
        protected virtual void ResetLeftGradient()
            => UpdateGradient(LeftGradient, UpdateLeftGradientColor, GetPreferredLeftTransparency());
        protected virtual void ResetRightGradient()
            => UpdateGradient(RightGradient, UpdateRightGradientColor, GetPreferredRightTransparency());

        protected virtual void UpdateGradients()
        {
            UpdateBottomGradient();
            UpdateTopGradient();
            UpdateLeftGradient();
            UpdateRightGradient();
        }
        protected virtual void UpdateBottomGradient()
            => UpdateGradient(BottomGradient, UpdateBottomGradientColor, GetNewCurrent(currentBottomTransparency, GetPreferredBottomTransparency()));
        protected virtual void UpdateTopGradient()
            => UpdateGradient(TopGradient, UpdateTopGradientColor, GetNewCurrent(currentTopTransparency, GetPreferredTopTransparency()));
        protected virtual void UpdateLeftGradient()
            => UpdateGradient(LeftGradient, UpdateLeftGradientColor, GetNewCurrent(currentLeftTransparency, GetPreferredLeftTransparency()));
        protected virtual void UpdateRightGradient()
            => UpdateGradient(RightGradient, UpdateRightGradientColor, GetNewCurrent(currentRightTransparency, GetPreferredRightTransparency()));

        protected virtual void UpdateGradient(Image gradient, UpdateGradientColor updateGradientColor, float transparency)
        {
            if (gradient != null)
                updateGradientColor(transparency);
        }

        protected delegate void UpdateGradientColor(float transparency);
        protected virtual void UpdateBottomGradientColor(float currentTransparency)
            => UpdateGradientImageColor(BottomGradient, currentBottomTransparency = currentTransparency);
        protected virtual void UpdateTopGradientColor(float currentTransparency)
            => UpdateGradientImageColor(TopGradient, currentTopTransparency = currentTransparency);
        protected virtual void UpdateLeftGradientColor(float currentTransparency)
            => UpdateGradientImageColor(LeftGradient, currentLeftTransparency = currentTransparency);
        protected virtual void UpdateRightGradientColor(float currentTransparency)
            => UpdateGradientImageColor(RightGradient, currentRightTransparency = currentTransparency);
        protected virtual void UpdateGradientImageColor(Image gradient, float transparency)
            => gradient.color = new Color(1, 1, 1, transparency);

        protected virtual float GetPreferredBottomTransparency()
            => ScrollRect.vertical ? GetPreferredVertical(ScrollRect.verticalNormalizedPosition) : -1;
        protected virtual float GetPreferredTopTransparency()
            => ScrollRect.vertical ? GetPreferredVertical(1 - ScrollRect.verticalNormalizedPosition) : -1;
        protected virtual float GetPreferredLeftTransparency()
            => ScrollRect.horizontal ? GetPreferredHorizontal(ScrollRect.horizontalNormalizedPosition) : -1;
        protected virtual float GetPreferredRightTransparency()
            => ScrollRect.horizontal ? GetPreferredHorizontal(1 - ScrollRect.horizontalNormalizedPosition) : -1;


        private const float ScrollAmount = .15f;
        protected virtual float GetPreferredVertical(float distFromEnd)
        {
            if (ScrollRect.content.rect.height <= ScrollRect.viewport.rect.height)
                return 0;
            else if (distFromEnd >= ScrollAmount)
                return 1;
            else
                return distFromEnd / ScrollAmount;
        }
        protected virtual float GetPreferredHorizontal(float distFromEnd)
        {
            if (ScrollRect.content.rect.width <= ScrollRect.viewport.rect.width)
                return 0;
            else if (distFromEnd >= ScrollAmount)
                return 1;
            else
                return distFromEnd / ScrollAmount;
        }

        private const float FadeTime = .2f;
        protected virtual float GetNewCurrent(float current, float preferred)
        {
            if (preferred < 0)
                return -1;
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
