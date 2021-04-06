namespace ClinicalTools.SimEncounters
{
    public interface IServerEncounterImageReader
    {
        WaitableTask GetTexture(User user, EncounterMetadata metadata, EncounterImage image);
    }
}
