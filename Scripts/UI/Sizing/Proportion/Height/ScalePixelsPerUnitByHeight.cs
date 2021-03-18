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

        private const float Tolerance = .0001f;
        private float height;
        private float scale;
        protected override void OnRectTransformDimensionsChange()
        {
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
            base.Awake();
            UpdateSize();
        }
        protected virtual void Update()
        {
            if (Texture != Image.sprite.texture)
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
            Image.pixelsPerUnitMultiplier = imageheight / height / scale;
        }
    }
}