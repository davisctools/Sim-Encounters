using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class SizeImageByScreenHeight : MonoBehaviour
    {
        private const float DefaultScreenHeight = 1030;

        public float Scale { get => scale; set => scale = value; }
        [SerializeField] private float scale = .5f;
        public bool SizeWidth { get => sizeWidth; set => sizeWidth = value; }
        [SerializeField] private bool sizeWidth = true;
        public bool SizeHeight { get => sizeHeight; set => sizeHeight = value; }
        [SerializeField] private bool sizeHeight = true;

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
        protected int ScreenHeight { get; set; }
        protected float LastScale { get; set; }
        protected float LossyScale { get; set; }
        protected virtual void Awake() => UpdateSize();
        protected virtual void Update()
        {
            if (RectTransform.lossyScale.y != LossyScale || Texture != Image.sprite.texture || ScreenHeight != Screen.height || LastScale != Scale)
                UpdateSize();
        }

        protected virtual void UpdateSize()
        {
            LossyScale = RectTransform.lossyScale.y;
            LastScale = Scale;
            ScreenHeight = Screen.height;
            Texture = Image.sprite.texture;
            if (Texture == null)
                return;

            var scaledScreenHeight = ScreenHeight / RectTransform.lossyScale.y;
            var heightProportion = scaledScreenHeight / DefaultScreenHeight;
            heightProportion *= Scale;
            var height = heightProportion * Texture.height;
            var width = heightProportion * Texture.width;
            if (SizeWidth)
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            if (SizeHeight)
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}