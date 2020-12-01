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

        public WaitableTask<Dictionary<int, Dictionary<SaveType, EncounterMetadata>>> GetMetadataGroups(User user)
        {
            var metadatasResults = new Dictionary<SaveType, WaitableTask<List<EncounterMetadata>>>();
#if DEMO
            if (metadatasReaders.ContainsKey(SaveType.Demo))
                metadatasResults.Add(SaveType.Demo, metadatasReaders[SaveType.Demo].GetMetadatas(user));
#else
            foreach (var metadatasReader in metadatasReaders.Where(r => r.Key != SaveType.Demo))
                metadatasResults.Add(metadatasReader.Key, metadatasReader.Value.GetMetadatas(user));
#endif
            var metadataGroups = new WaitableTask<Dictionary<int, Dictionary<SaveType, EncounterMetadata>>>();

            foreach (var metadatasResult in metadatasResults)
                metadatasResult.Value.AddOnCompletedListener((result) => ProcessResult(metadataGroups, metadatasResults));

            return metadataGroups;
        }

        private void ProcessResult(WaitableTask<Dictionary<int, Dictionary<SaveType, EncounterMetadata>>> result,
            Dictionary<SaveType, WaitableTask<List<EncounterMetadata>>> metadatasResults)
        {
            if (result.IsCompleted())
                return;

            foreach (var metadatasResult in metadatasResults) {
                if (!metadatasResult.Value.IsCompleted())
                    return;
            }

            var metadataGroups = new Dictionary<int, Dictionary<SaveType, EncounterMetadata>>();
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

        private void AddMetadata(Dictionary<int, Dictionary<SaveType, EncounterMetadata>> metadataGroups, 
            SaveType saveType, EncounterMetadata metadata)
        {
            Dictionary<SaveType, EncounterMetadata> metadataGroup;
            if (metadataGroups.ContainsKey(metadata.RecordNumber)) {
                metadataGroup = metadataGroups[metadata.RecordNumber];
            } else {
                metadataGroup = new Dictionary<SaveType, EncounterMetadata>();
                metadataGroups.Add(metadata.RecordNumber, metadataGroup);
            }

            if (!metadataGroup.ContainsKey(saveType))
                metadataGroup.Add(saveType, metadata);
        }
    }
}