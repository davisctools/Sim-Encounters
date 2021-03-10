using UnityEngine;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FitInSafeArea : MonoBehaviour
    {
        protected virtual void Update()
        {
            var rectTransform = (RectTransform)transform;
            var safeArea = Screen.safeArea;
            
            var minAnchor = new Vector2 {
                x = safeArea.x / Screen.width,
                y = (safeArea.y + AndroidStatusBarManager.NavigationBarHeight) / Screen.height
            };

            var maxAnchor = new Vector2 {
                x = (safeArea.x + safeArea.width) / Screen.width,
                y = (safeArea.y + safeArea.height) / Screen.height
            };

            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
        }
    }
}