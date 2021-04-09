namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageReader
    {
        WaitableTask GetTexture(User user, EncounterMetadata metadata, EncounterImage image);
    }
}
