using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterFileReader
    {
        WaitableTask<string> ReadTextFile(User user, OldEncounterMetadata metadata, EncounterDataFileType fileType);
        WaitableTask<Texture2D> ReadTextureFile(User user, OldEncounterMetadata metadata, string filename);
        WaitableTask<string[]> ReadTextFiles(User user, EncounterDataFileType fileType);
    }
}