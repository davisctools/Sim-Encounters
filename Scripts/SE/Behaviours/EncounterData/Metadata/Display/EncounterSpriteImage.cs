using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class EncounterSpriteImage : EncounterMetadataBehaviour
    {
        protected Image Image => (image == null) ? image = GetComponent<Image>() : image;
        private Image image;

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs) 
            => Image.sprite = eventArgs.Metadata.Sprite;
    }
}