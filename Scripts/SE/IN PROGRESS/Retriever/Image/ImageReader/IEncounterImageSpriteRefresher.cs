namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageSpriteRefresher
    {
        WaitableTask RefreshTexture(User user, OldEncounterMetadata metadata, EncounterImage image);
    }
}
