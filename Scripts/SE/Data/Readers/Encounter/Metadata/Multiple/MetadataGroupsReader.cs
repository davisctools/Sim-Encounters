using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MetadataGroupsReader : IMetadataGroupsReader
    {
        private readonly Dictionary<SaveType, IMetadatasReader> metadatasReaders = new Dictionary<SaveType, IMetadatasReader>();
        public MetadataGroupsReader(
            [Inject(Id = SaveType.Autosave)] IMetadatasReader autosaveMetadataReader,
            [Inject(Id = SaveType.Demo)] IMetadatasReader demoMetadatasReader,
            [Inject(Id = SaveType.Local)] IMetadatasReader localMetadataReader,
            [Inject(Id = SaveType.Server)] IMetadatasReader serverMetadatasReader)
        {
            if (autosaveMetadataReader != null)
                metadatasReaders.Add(SaveType.Autosave, autosaveMetadataReader);
            if (localMetadataReader != null)
                metadatasReaders.Add(SaveType.Local, localMetadataReader);
            if (serverMetadatasReader != null)
                metadatasReaders.Add(SaveType.Server, serverMetadatasReader);
            if (demoMetadatasReader != null)
                metadatasReaders.Add(SaveType.Demo, demoMetadatasReader);
        }

        public WaitableTask<Dictionary<int, Dictionary<SaveType, OldEncounterMetadata>>> GetMetadataGroups(User user)
        {
            var metadatasResults = new Dictionary<SaveType, WaitableTask<List<OldEncounterMetadata>>>();
#if DEMO
            if (metadatasReaders.ContainsKey(SaveType.Demo))
                metadatasResults.Add(SaveType.Demo, metadatasReaders[SaveType.Demo].GetMetadatas(user));
#else
            foreach (var metadatasReader in metadatasReaders.Where(r => r.Key != SaveType.Demo))
                metadatasResults.Add(metadatasReader.Key, metadatasReader.Value.GetMetadatas(user));
#endif
            var metadataGroups = new WaitableTask<Dictionary<int, Dictionary<SaveType, OldEncounterMetadata>>>();

            foreach (var metadatasResult in metadatasResults)
                metadatasResult.Value.AddOnCompletedListener((result) => ProcessResult(metadataGroups, metadatasResults));

            return metadataGroups;
        }

        private void ProcessResult(WaitableTask<Dictionary<int, Dictionary<SaveType, OldEncounterMetadata>>> result,
            Dictionary<SaveType, WaitableTask<List<OldEncounterMetadata>>> metadatasResults)
        {
            if (result.IsCompleted())
                return;

            foreach (var metadatasResult in metadatasResults) {
                if (!metadatasResult.Value.IsCompleted())
                    return;
            }

            var metadataGroups = new Dictionary<int, Dictionary<SaveType, OldEncounterMetadata>>();
            foreach (var metadatasResult in metadatasResults) {
                if (!metadatasResult.Value.Result.HasValue())
                    continue;

                foreach (var metadata in metadatasResult.Value.Result.Value)
                    AddMetadata(metadataGroups, metadatasResult.Key, metadata);
            }

            // Server metadata filename should be changed to match the local filename
            // This ensures that the old files are properly renamed if using the server version
            foreach (var metadataGroup in metadataGroups) {
                var group = metadataGroup.Value;
                if (!group.ContainsKey(SaveType.Local) || !group.ContainsKey(SaveType.Server))
                    continue;

                group[SaveType.Server].Filename = group[SaveType.Local].Filename;
            }

            result.SetResult(metadataGroups);
        }

        private void AddMetadata(Dictionary<int, Dictionary<SaveType, OldEncounterMetadata>> metadataGroups, 
            SaveType saveType, OldEncounterMetadata metadata)
        {
            Dictionary<SaveType, OldEncounterMetadata> metadataGroup;
            if (metadataGroups.ContainsKey(metadata.RecordNumber)) {
                metadataGroup = metadataGroups[metadata.RecordNumber];
            } else {
                metadataGroup = new Dictionary<SaveType, OldEncounterMetadata>();
                metadataGroups.Add(metadata.RecordNumber, metadataGroup);
            }

            if (!metadataGroup.ContainsKey(saveType))
                metadataGroup.Add(saveType, metadata);
        }
    }
}