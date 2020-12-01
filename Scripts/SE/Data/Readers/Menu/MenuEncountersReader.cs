
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncountersReader : IMenuEncountersReader
    {
        private readonly IMetadataGroupsReader metadataGroupsReader;
        private readonly IBasicStatusesReader basicStatusesReader;
        public MenuEncountersReader(IMetadataGroupsReader metadataGroupsReader, IBasicStatusesReader basicStatusesReader)
        {
            this.metadataGroupsReader = metadataGroupsReader;
            this.basicStatusesReader = basicStatusesReader;
        }

        public WaitableTask<List<MenuEncounter>> GetMenuEncounters(User user)
        {
            var metadataGroups = metadataGroupsReader.GetMetadataGroups(user);
            var statuses = basicStatusesReader.GetBasicStatuses(user);

            var menuEncounters = new WaitableTask<List<MenuEncounter>>();

            void processResults() => ProcessResults(menuEncounters, metadataGroups, statuses);
            metadataGroups.AddOnCompletedListener((result) => processResults());
            statuses.AddOnCompletedListener((result) => processResults());

            return menuEncounters;
        }

        protected void ProcessResults(WaitableTask<List<MenuEncounter>> result,
            WaitableTask<Dictionary<int, Dictionary<SaveType, EncounterMetadata>>> metadataGroups,
            WaitableTask<Dictionary<int, EncounterBasicStatus>> statuses)
        {
            if (result.IsCompleted() || !metadataGroups.IsCompleted() || !statuses.IsCompleted())
                return;

            if (metadataGroups.Result.Value == null)
            {
                result.SetError(null);
                return;
            }

            var menuEncounters = new List<MenuEncounter>();
            foreach (var metadataGroup in metadataGroups.Result.Value)
                menuEncounters.Add(GetMenuEncounter(metadataGroup, statuses));
            result.SetResult(menuEncounters);
        }

        protected MenuEncounter GetMenuEncounter(KeyValuePair<int, Dictionary<SaveType, EncounterMetadata>> metadataGroup,
            WaitableTask<Dictionary<int, EncounterBasicStatus>> statuses)
        {
            EncounterBasicStatus status = null;
            if (statuses.Result.Value?.ContainsKey(metadataGroup.Key) == true)
                status = statuses.Result.Value[metadataGroup.Key];
            return new MenuEncounter(metadataGroup.Value, status);
        }
    }
}