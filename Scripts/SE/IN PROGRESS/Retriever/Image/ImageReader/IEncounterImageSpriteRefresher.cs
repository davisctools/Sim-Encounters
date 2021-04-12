namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageSpriteRefresher
    {
        WaitableTask RefreshTexture(User user, EncounterMetadata metadata, EncounterImage image);
    }
}
