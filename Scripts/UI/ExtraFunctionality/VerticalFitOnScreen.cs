using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class VerticalFitOnScreen : MonoBehaviour
    {
        private RectTransform rectTrans;
        [SerializeField] private VerticalLayoutGroup content = null;
        private RectTransform contentTrans;

        private void Awake()
        {
            rectTrans = (RectTransform)transform;
            contentTrans = (RectTransform)content.transform;
        }

        private void OnEnable()
        {
            var corners = new Vector3[4];
            ((RectTransform)transform).GetWorldCorners(corners);
            if (corners[0].y < 0)
                MoveToTop();
        }

        private void OnDisable()
        {
            MoveToBottom();
        }

        private void MoveToTop()
        {
            rectTrans.anchorMin = new Vector2(0, 1);
            rectTrans.anchorMax = new Vector2(1, 1);
            rectTrans.pivot = new Vector2(.5f, 0);

            contentTrans.anchorMin = new Vector2(0, 0);
            contentTrans.anchorMax = new Vector2(1, 0);
            contentTrans.pivot = new Vector2(.5f, 0);
        }

        private void MoveToBottom()
        {
            rectTrans.anchorMin = new Vector2(0, 0);
            rectTrans.anchorMax = new Vector2(1, 0);
            rectTrans.pivot = new Vector2(.5f, 1);

            contentTrans.anchorMin = new Vector2(0, 1);
            contentTrans.anchorMax = new Vector2(1, 1);
            contentTrans.pivot = new Vector2(.5f, 1);
        }
    }
}