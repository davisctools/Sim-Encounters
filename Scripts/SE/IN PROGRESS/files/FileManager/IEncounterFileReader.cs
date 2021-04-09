using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterFileReader
    {
        WaitableTask<string> ReadTextFile(User user, EncounterMetadata metadata, EncounterDataFileType fileType);
        WaitableTask<Texture2D> ReadTextureFile(User user, EncounterMetadata metadata, string filename);
        WaitableTask<string[]> ReadTextFiles(User user, EncounterDataFileType fileType);
    }
}