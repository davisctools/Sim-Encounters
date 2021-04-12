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

        protected Image Image => (image == null) ? image = GetComponent<Image>() : image;
        private Image image;

        protected WriterSceneInfo SceneInfo => SceneInfoSelectedListener.CurrentValue.SceneInfo;
        protected Encounter Encounter => SceneInfo.Encounter;
        protected User User => SceneInfo.User;
        protected IEncounterImageSelector ImageSelector { get; set; }
        protected ISelectedListener<WriterSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            IEncounterImageSelector imageSelector,
            ISelectedListener<WriterSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            ImageSelector = imageSelector;
            SceneInfoSelectedListener = sceneInfoSelectedListener;
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(this, PanelSelectedListener.CurrentValue);
        }

        protected virtual void Awake() => SelectImageButton.onClick.AddListener(SelectImage);

        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.LegacyValues;
            if (values.ContainsKey(Name))
                SetEncounterImage(GetImage(values[Name]));
        }

        protected EncounterImage CurrentImage { get; set; }
        public virtual void SetEncounterImage(EncounterImage encounterImage)
        {
            if (CurrentImage != null)
                CurrentImage.Updated -= SetEncounterImage;
            CurrentImage = encounterImage;
            if (CurrentImage != null)
                CurrentImage.Updated += SetEncounterImage;

            value = encounterImage?.Key;

            Image.sprite = encounterImage?.Sprite;
            Color imageColor = encounterImage?.Sprite != null ? Color.white : Color.clear;

            Image.color = imageColor;
            Image.enabled = encounterImage?.Key != null;
        }

        protected virtual string LegacyEncounterImageKey => "patientImage";
        protected virtual string EncounterImageKey => "encounterImage";
        protected virtual EncounterImage GetImage(string imageKey)
        {
            if (imageKey == null)
                return null;

            if (KeyIsEncounterImage(imageKey))
                return Encounter.Metadata.Image;

            var images = Encounter.Content.Images;
            if (images.ContainsKey(imageKey))
                return images[imageKey];

            return null;
        }

        protected virtual bool KeyIsEncounterImage(string key)
            => key != null && (key.Equals(LegacyEncounterImageKey, StringComparison.InvariantCultureIgnoreCase) || key.Equals(EncounterImageKey, StringComparison.InvariantCultureIgnoreCase));

        protected virtual void SelectImage()
        {
            var newImage = ImageSelector.SelectImage(User, Encounter, Value);
            newImage.AddOnCompletedListener(ImageSelected);
        }

        protected virtual void ImageSelected(TaskResult<EncounterImage> encounterImage)
        {
            if (!encounterImage.IsError())
                SetEncounterImage(encounterImage.Value);
        }

        protected virtual void OnDestroy()
        {
            if (CurrentImage != null)
                CurrentImage.Updated -= SetEncounterImage;
        }
    }
}