namespace ClinicalTools.SimEncounters
{
    public class LocalEncounterDataTextRetriever : IEncounterDataTextRetriever
    {
        protected IEncounterFileReader FileReader { get; }
        public LocalEncounterDataTextRetriever(IEncounterFileReader fileReader) => FileReader = fileReader;

        public virtual WaitableTask<string> GetDataText(User user, OldEncounterMetadata metadata)
            => FileReader.ReadTextFile(user, metadata, EncounterDataFileType.Data);
    }
}