using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class WriterPanelImage : MonoBehaviour, IWriterPanelField
    {
        public virtual string Name => name;
        public virtual string Value => value;
        private string value = null;

        public Button SelectImageButton { get => selectImageButton; set => selectImageButton = value; }
        [SerializeField] private Button selectImageButton;

        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }
        private Image image;


        protected Encounter Encounter => EncounterSelectedListener.CurrentValue.Encounter;
        protected IEncounterSpriteSelector SpritePopup { get; set; }
        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            IEncounterSpriteSelector spritePopup,
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener,
            ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            SpritePopup = spritePopup;
            EncounterSelectedListener = encounterSelectedListener;
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(this, PanelSelectedListener.CurrentValue);
        }

        protected virtual void Awake() => SelectImageButton.onClick.AddListener(SelectImage);

        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            if (values.ContainsKey(Name))
                SetSprite(values[Name]);
        }

        public virtual void SetSprite(string imageKey)
        {
            value = imageKey;

            var sprite = GetSprite(imageKey);
            Color imageColor;
            if (sprite != null) {
                Image.sprite = sprite;
                imageColor = Color.white;
            } else {
                imageColor = Color.clear;
            }

            Image.color = imageColor;
            Image.enabled = imageKey != null;
        }

        protected virtual string LegacyEncounterImageKey => "patientImage";
        protected virtual string EncounterImageKey => "encounterImage";
        protected virtual Sprite GetSprite(string imageKey)
        {
            if (imageKey == null)
                return null;

            if (KeyIsEncounterImage(imageKey))
                return Encounter.Metadata.Sprite;

            var sprites = Encounter.Content.ImageContent.Sprites;
            if (sprites.ContainsKey(imageKey))
                return sprites[imageKey];

            return null;
        }

        protected virtual bool KeyIsEncounterImage(string key)
            => key != null && (key.Equals(LegacyEncounterImageKey, StringComparison.InvariantCultureIgnoreCase) || key.Equals(EncounterImageKey, StringComparison.InvariantCultureIgnoreCase));

        protected virtual void SelectImage()
        {
            var newImageKey = SpritePopup.SelectSprite(Encounter, Value);
            newImageKey.AddOnCompletedListener(ImageSelected);
        }

        protected virtual void ImageSelected(TaskResult<string> imageKey)
        {
            if (!imageKey.IsError())
                SetSprite(imageKey.Value);
        }
    }
}