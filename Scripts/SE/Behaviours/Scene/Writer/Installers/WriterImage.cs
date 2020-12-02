using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class WriterImage : MonoBehaviour, IWriterPanelField
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
        protected IKeyedSpriteSelector SpritePopup { get; set; }
        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            IKeyedSpriteSelector spritePopup,
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

        private const string PatientImageKey = "patientImage";
        protected virtual Sprite GetSprite(string imageKey)
        {
            if (imageKey == null)
                return null;

            if (imageKey.Equals(PatientImageKey, StringComparison.InvariantCultureIgnoreCase))
                return Encounter.Metadata.Sprite;

            var sprites = Encounter.Content.ImageContent.Sprites;
            if (sprites.ContainsKey(imageKey))
                return sprites[imageKey];

            return null;
        }

        protected virtual void SelectImage()
        {
            var newImageKey = SpritePopup.SelectSprite(Encounter.Content.ImageContent.Sprites, Value);
            newImageKey.AddOnCompletedListener(ImageSelected);
        }

        protected virtual void ImageSelected(TaskResult<string> imageKey)
        {
            if (!imageKey.IsError())
                SetSprite(imageKey.Value);
        }
    }
}