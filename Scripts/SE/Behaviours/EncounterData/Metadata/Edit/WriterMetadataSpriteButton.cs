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

        protected ISpriteSelector SpriteSelector { get; set; }
        [Inject] public virtual void Inject(ISpriteSelector spriteSelector) => SpriteSelector = spriteSelector;

        protected override void Start()
        {
            base.Start();
            Button.onClick.AddListener(SelectSprite);
        }

        protected virtual void SelectSprite()
        {
            var spriteTask = SpriteSelector.SelectSprite(MetadataSelector.CurrentValue.Metadata.Image.Sprite);
            spriteTask.AddOnCompletedListener(SpriteSelected);
        }
        protected virtual void SpriteSelected(TaskResult<Sprite> spriteResult)
        {
            if (spriteResult.IsError())
                return;
            Image.sprite = spriteResult.Value;
            Serialize(MetadataSelector.CurrentValue.Metadata);
        }
        protected override void Serialize(EncounterMetadata metadata)
            => metadata.Image.Sprite = Image.sprite;

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => Image.sprite = eventArgs.Metadata.Image.Sprite;
    }
}