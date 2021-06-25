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
        private float referencePixelsPerUnit;
        protected override void OnRectTransformDimensionsChange()
        {
            if (canvas == null)
                return;

            base.OnRectTransformDimensionsChange();

            UpdateSizeIfValueChanged();
        }

        protected bool IsWithinTolerance(float num1, float num2)
            => Mathf.Abs(num1 - num2) <= Tolerance;

        protected Texture2D Texture { get; set; }
        protected float LastScale { get; set; }
        protected override void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            base.Awake();
            UpdateSize();
        }

        protected override void Start()
        {
            base.Start();
            if (canvas == null) {
                canvas = GetComponentInParent<Canvas>();
                UpdateSize();
                // this is messy but the reference pixels needs to be a big enough change for this to actually update properly
                canvas.referencePixelsPerUnit -= .01f;
                NextFrame.Function(UpdateSize);
            }
        }

        protected virtual void Update() => UpdateSizeIfValueChanged();

        protected void UpdateSizeIfValueChanged()
        {
            bool valueChanged = false;

            var currentHeight = RectTransform.rect.height;
            if (!IsWithinTolerance(currentHeight, height)) {
                height = currentHeight;
                valueChanged = true;
            }

            var currentScale = RectTransform.lossyScale.y / RectTransform.lossyScale.x;
            if (!IsWithinTolerance(currentScale, scale)) {
                scale = currentScale;
                valueChanged = true;
            }

            if (Texture != Image.sprite.texture) {
                Texture = Image.sprite.texture;
                valueChanged = true;
            }

            if (canvas != null && !IsWithinTolerance(canvas.referencePixelsPerUnit, referencePixelsPerUnit)) {
                referencePixelsPerUnit = canvas.referencePixelsPerUnit;
                valueChanged = true;
            }

            if (valueChanged)
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
            if (canvas != null) {
                referencePixelsPerUnit = canvas.referencePixelsPerUnit;
                Image.pixelsPerUnitMultiplier = imageheight / height / scale * referencePixelsPerUnit / 100;
            }
        }
    }
}