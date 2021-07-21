using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FitInSafeArea : MonoBehaviour
    {
        public bool AccountForNavigationBar { get => accountForNavigationBar; set => accountForNavigationBar = value; }
        [SerializeField] private bool accountForNavigationBar = true;

        protected virtual void Update()
        {
            var rectTransform = (RectTransform)transform;
            var safeArea = Screen.safeArea;

            var minAnchor = new Vector2 {
                x = safeArea.x / Screen.width,
                y = (safeArea.y + GetBottomOffset()) / Screen.height
            };

            var maxAnchor = new Vector2 {
                x = (safeArea.x + safeArea.width) / Screen.width,
                y = (safeArea.y + safeArea.height) / Screen.height
            };

            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
        }

        protected virtual float GetBottomOffset() => (AccountForNavigationBar) ? (AndroidStatusBarManager.NavigationBarHeight) : 0;

    }
}