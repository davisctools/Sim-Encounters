using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class MobilePanelImage : UIBehaviour
    {
        private float width;

        private const float Tolerance = .0001f;
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var currentWidth = GetWidth();
            if (Mathf.Abs(currentWidth - width) < Tolerance)
                return;

            width = currentWidth;
            UpdateHeight();
        }

        public string Name => name;
        public string Value { get; private set; } = null;

        public GameObject ImageGroup { get => imageGroup; set => imageGroup = value; }
        [SerializeField] private GameObject imageGroup;
        public float MaxHeight { get => maxHeight; set => maxHeight = value; }
        [SerializeField] private float maxHeight = -1;
        public LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }
        [SerializeField] private LayoutElement layoutElement;
        public Button EnlargeImageButton { get => enlargeImageButton; set => enlargeImageButton = value; }
        [SerializeField] private Button enlargeImageButton;
        public bool UseEncounterImage { get => useEncounterImage; set => useEncounterImage = value; }
        [SerializeField] private bool useEncounterImage;
        
        protected Image Image => (image == null) ? image = GetComponent<Image>() : image;
        private Image image;

        protected CanvasResizer CanvasResizer { get; set; }
        protected SpriteDrawer SpritePopup { get; set; }
        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            CanvasResizer canvasResizer,
            SpriteDrawer spritePopup,
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener,
            ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            CanvasResizer = canvasResizer;
            SpritePopup = spritePopup;
            EncounterSelectedListener = encounterSelectedListener;
            PanelSelectedListener = panelSelectedListener;
        }

        protected override void Start()
        {
            base.Start();
            CanvasResizer.Resized += UpdateLayoutElement;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(PanelSelectedListener, PanelSelectedListener.CurrentValue);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            PanelSelectedListener.Selected -= OnPanelSelected;
        }

        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs eventArgs)
        {
            var sprite = GetSprite(eventArgs);
            if (sprite != null)
                SetSprite(sprite);
            else
                HideImage();
        }

        protected virtual string LegacyEncounterImageKey => "patientImage";
        protected virtual string EncounterImageKey => "encounterImage";
        protected virtual Sprite GetSprite(PanelSelectedEventArgs eventArgs)
        {
            if (UseEncounterImage)
                return EncounterSelectedListener.CurrentValue.Encounter.Metadata.Sprite;

            if (!eventArgs.Panel.Values.ContainsKey(Name))
                return null;

            Value = eventArgs.Panel.Values[Name];

            if (KeyIsEncounterImage(Value))
                return EncounterSelectedListener.CurrentValue.Encounter.Metadata.Sprite;

            var sprites = EncounterSelectedListener.CurrentValue.Encounter.Content.ImageContent.Sprites;
            if (Value != null && sprites.ContainsKey(Value))
                return sprites[Value];
            else
                return null;
        }

        protected virtual bool KeyIsEncounterImage(string key)
            => key != null 
                && (key.Equals(LegacyEncounterImageKey, StringComparison.InvariantCultureIgnoreCase)
                    || key.Equals(EncounterImageKey, StringComparison.InvariantCultureIgnoreCase));

        protected virtual void HideImage()
        {
            if (ImageGroup != null)
                ImageGroup.SetActive(false);
        }

        protected Sprite Sprite { get; set; }
        protected virtual void SetSprite(Sprite sprite)
        {
            if (sprite == null) {
                Debug.LogError("Sprite is null");
                return;
            }

            Image.sprite = sprite;
            Sprite = sprite;

            UpdateHeight();
        }

        protected virtual float GetWidth()
        {
            var rectWidth = ((RectTransform)transform).rect.width;
            return (CanvasResizer != null) ? rectWidth / CanvasResizer.ResizeValue : rectWidth;
        }

        private Vector2 unscaledDimensions = new Vector2();
        protected virtual void UpdateHeight()
        {
            if (Sprite == null)
                return;

            if (width == 0)
                width = GetWidth();

            var spriteHeight = Sprite.rect.height;
            var spriteWidth = Sprite.rect.width;
            var spriteRatio = spriteHeight / spriteWidth;

            if (EnlargeImageButton != null) {
                EnlargeImageButton.onClick.RemoveAllListeners();
                EnlargeImageButton.onClick.AddListener(() => SpritePopup.Display(Image.sprite));
            }

            var height = width * spriteRatio;
            if (MaxHeight > Tolerance && MaxHeight < height) {
                unscaledDimensions.y = MaxHeight;
                unscaledDimensions.x = MaxHeight / spriteRatio;
            } else {
                unscaledDimensions.y = height;
                unscaledDimensions.x = width;
            }


            UpdateLayoutElement(CanvasResizer.ResizeValue);
        }

        protected virtual void UpdateLayoutElement(float scale)
        {
            if (LayoutElement == null)
                return;

            LayoutElement.preferredHeight = unscaledDimensions.y * scale;

            if (unscaledDimensions.x <= 0)
                return;

            LayoutElement.preferredWidth = unscaledDimensions.x * scale;
            LayoutElement.flexibleWidth = -1;
        }
    }
}