using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class SizeImageToFitInParent : UIBehaviour
    {
        public RectTransform ParentTransform { get => parentTransform; set => parentTransform = value; }
        [SerializeField] private RectTransform parentTransform;
        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
        public LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }
        [SerializeField] private LayoutElement layoutElement;


        protected Texture2D Texture { get; set; }
        protected override void Awake()
        {
            base.Awake();
            UpdateSize();
        }

        protected override void Start()
        {
            base.Start();
            Update();
        }
        protected virtual void Update()
        {
            if (Image.sprite == null || Texture == Image.sprite.texture)
                return;

            UpdateSize();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            UpdateSize();
        }

        protected virtual void UpdateSize()
        {
            if (Image.sprite == null)
                return;
            Texture = Image.sprite.texture;
            if (Texture == null)
                return;

            var rect = ParentTransform.rect;
            var textureScale = Texture.width / Texture.height;
            var rectScale = rect.width / rect.height;

            if (rectScale > textureScale) {
                LayoutElement.flexibleWidth = -1;
                LayoutElement.preferredWidth = rect.height * textureScale;
            } else if (rectScale < textureScale) {
                LayoutElement.flexibleHeight = -1;
                LayoutElement.preferredHeight = rect.width / textureScale;
            }

            SetDirty(ParentTransform);
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