using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class LocalEncounterImageTextureRetriever : IEncounterImageTextureRetriever
    {
        protected IEncounterFileReader FileReader { get; }
        public LocalEncounterImageTextureRetriever(IEncounterFileReader fileReader) => FileReader = fileReader;

        public WaitableTask<Texture2D> GetTexture(User user, OldEncounterMetadata metadata, EncounterImage image)
            => FileReader.ReadTextureFile(user, metadata, image.Filename);
    }
}
