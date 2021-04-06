using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class WriterMetadataSpriteButton : WriterMetadataBehaviour
    {
        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;

        protected Button Button => (button == null) ? button = GetComponent<Button>() : button;
        private Button button;

        protected ISelectedListener<WriterSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected IEncounterImageSelector ImageSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<WriterSceneInfoSelectedEventArgs> sceneInfoSelectedListener, IEncounterImageSelector imageSelector)
        {
            SceneInfoSelectedListener = sceneInfoSelectedListener;
            ImageSelector = imageSelector;
        }

        protected WriterSceneInfo SceneInfo => SceneInfoSelectedListener.CurrentValue.SceneInfo;

        protected override void Start()
        {
            base.Start();
            Button.onClick.AddListener(SelectSprite);
        }

        protected virtual void SelectSprite()
        {
            var spriteTask = ImageSelector.SelectImage(SceneInfo.User, SceneInfo.Encounter, CurrentImage?.Key);
            spriteTask.AddOnCompletedListener(SpriteSelected);
        }
        protected EncounterImage CurrentImage { get; set; }
        protected virtual void SpriteSelected(TaskResult<EncounterImage> imageResult)
        {
            if (!imageResult.IsError())
                SetEncounterImage(imageResult.Value);
        }
        protected override void Serialize(EncounterMetadata metadata) => MetadataSelector.CurrentValue.Metadata.Image = CurrentImage;

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs) => SetEncounterImage(eventArgs.Metadata.Image);

        public virtual void SetEncounterImage(EncounterImage encounterImage)
        {
            if (CurrentImage != null)
                CurrentImage.Updated -= SetEncounterImage;
            CurrentImage = encounterImage;
            if (CurrentImage != null)
                CurrentImage.Updated += SetEncounterImage;

            Image.sprite = CurrentImage?.Sprite;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (CurrentImage != null)
                CurrentImage.Updated -= SetEncounterImage;
        }
    }
}