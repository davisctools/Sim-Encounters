using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class ScalePixelsPerUnitByHeight : UIBehaviour
    {
        protected RectTransform RectTransform => (RectTransform)transform;

        private Image image;
        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        private Canvas canvas;
        private const float Tolerance = .0001f;
        private float height;
        private float scale;
        protected override void OnRectTransformDimensionsChange()
        {
            if (canvas == null)
                return;

            base.OnRectTransformDimensionsChange();

            var currentHeight = RectTransform.rect.height;
            var currentScale = RectTransform.lossyScale.y / RectTransform.lossyScale.x;
            if (Mathf.Abs(currentHeight - height) < Tolerance
                && Mathf.Abs(currentScale - scale) < Tolerance) {
                return;
            }

            height = currentHeight;
            scale = currentScale;
            UpdateSize();
        }

        protected Texture2D Texture { get; set; }
        protected float LastScale { get; set; }
        protected override void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            base.Awake();
            UpdateSize();
        }
        protected virtual void Update()
        {
            var currentHeight = RectTransform.rect.height;
            var currentScale = RectTransform.lossyScale.y / RectTransform.lossyScale.x;
            if (Texture == Image.sprite.texture 
                && Mathf.Abs(currentHeight - height) < Tolerance
                && Mathf.Abs(currentScale - scale) < Tolerance) {
                return;
            }

            height = currentHeight;
            scale = currentScale;
            UpdateSize();
        }

        protected virtual void UpdateSize()
        {
            Texture = Image.sprite.texture;
            if (Texture == null)
                return;

            if (height == 0)
                height = RectTransform.rect.height;

            scale = RectTransform.lossyScale.y / RectTransform.lossyScale.x;

            var imageheight = Texture.height;
            // TODO: This is updated when the image scale is changed, but it doesn't always update how the image looks
            // Updating on later frames also doesn't fix the issue
            Image.pixelsPerUnitMultiplier = imageheight / height / scale * canvas.referencePixelsPerUnit / 100;
        }
    }
}