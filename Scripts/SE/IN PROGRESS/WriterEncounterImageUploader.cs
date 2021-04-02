using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterImageUploader : MonoBehaviour, IEncounterImageUploader, IEncounterImageUpdater
    {
        public WaitableTask<EncounterImage> UpdateImage(User user, Encounter encounter, EncounterImage image)
        {
            return new WaitableTask<EncounterImage>();
        }

        public virtual WaitableTask<EncounterImage> UploadImage(User user, Encounter encounter)
        {
            return new WaitableTask<EncounterImage>();
        }
    }
}