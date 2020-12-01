using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class EncounterSpriteImage : EncounterMetadataBehaviour
    {
        private Image image;
        protected Image Image
        {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs) 
            => Image.sprite = eventArgs.Metadata.Sprite;
    }
}