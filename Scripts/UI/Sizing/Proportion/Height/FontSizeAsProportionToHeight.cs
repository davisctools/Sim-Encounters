using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{

    [ExecuteAlways]
    public class FontSizeAsProportionToHeight : UIBehaviour
    {
        public float FontSizePerHeight { get => fontSizePerHeight; set => fontSizePerHeight = value; }
        [SerializeField] private float fontSizePerHeight = 1;

        private TMP_Text text;
        protected TMP_Text Text
        {
            get {
                if (text == null)
                    text = GetComponent<TMP_Text>();
                return text;
            }
        }
        protected LayoutGroup parentGroup;

        private const float Tolerance = .0001f;
        private float height;

        protected override void Awake()
        {
            base.Awake();
            if (transform.parent != null)
                parentGroup = transform.parent.GetComponent<LayoutGroup>();
            UpdateFontSize();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var currentHeight = ((RectTransform)transform).rect.height;
            if (Mathf.Abs(currentHeight - height) < Tolerance)
                return;

            height = currentHeight;
            UpdateFontSize();
        }

        private float lastFontSizePerHeight;
        protected void Update()
        {
            if (lastFontSizePerHeight != FontSizePerHeight)
                UpdateFontSize();
        }

        protected virtual void UpdateFontSize()
        {
            if (height < Tolerance)
                height = ((RectTransform)transform).rect.height;

            lastFontSizePerHeight = FontSizePerHeight;
            Text.fontSize = FontSizePerHeight * height;

            if (parentGroup != null)
                SetDirty((RectTransform)parentGroup.transform);
        }

        protected void SetDirty(RectTransform rectTransform)
        {
            if (!IsActive())
                return;

            if (!CanvasUpdateRegistry.IsRebuildingLayout())
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            else
                StartCoroutine(DelayedSetDirty(rectTransform));
        }

        protected IEnumerator DelayedSetDirty(RectTransform rectTransform)
        {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}