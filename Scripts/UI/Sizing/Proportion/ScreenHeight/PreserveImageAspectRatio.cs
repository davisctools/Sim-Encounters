using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class PreserveImageAspectRatio : UIBehaviour
    {
        public bool KeepHeight { get => keepHeight; set => keepHeight = value; }
        [SerializeField] private bool keepHeight = true;

        protected RectTransform RectTransform => (RectTransform)transform;

        private Image image;
        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected Texture2D Texture { get; set; }
        protected Vector3 LossyScale { get; set; }
        protected override void Awake()
        {
            UpdateSize();
            base.Awake();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            UpdateSize();
        }

        protected virtual void Update()
        {
            if (RectTransform.lossyScale != LossyScale || Texture != Image.sprite.texture)
                UpdateSize();
        }

        protected virtual void UpdateSize()
        {
            LossyScale = RectTransform.lossyScale;
            Texture = Image.sprite.texture;
            if (Texture == null)
                return; 
            var rect = RectTransform.rect;
            var aspectRatio = 1f * Texture.width / Texture.height;

            if (KeepHeight)
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.height * aspectRatio / LossyScale.x);
            else
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.width / aspectRatio / LossyScale.y);
        }
    }
}