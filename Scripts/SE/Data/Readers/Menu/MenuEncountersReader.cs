using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncountersReader : IMenuEncountersReader
    {
        private readonly IEncounterLocksReader locksReader;
        private readonly IMetadataGroupsReader metadataGroupsReader;
        private readonly IBasicStatusesReader basicStatusesReader;
        public MenuEncountersReader(
            IEncounterLocksReader locksReader, 
            IMetadataGroupsReader metadataGroupsReader, 
            IBasicStatusesReader basicStatusesReader)
        {
            this.locksReader = locksReader;
            this.metadataGroupsReader = metadataGroupsReader;
            this.basicStatusesReader = basicStatusesReader;
        }

        public virtual WaitableTask<List<MenuEncounter>> GetMenuEncounters(User user)
        {
            var metadataGroups = metadataGroupsReader.GetMetadataGroups(user);
            var locks = locksReader.GetEncounterLocks(user);
            var statuses = basicStatusesReader.GetBasicStatuses(user);

            var menuEncounters = new WaitableTask<List<MenuEncounter>>();

            void processResults() => ProcessResults(menuEncounters, metadataGroups, locks, statuses);
            metadataGroups.AddOnCompletedListener((result) => processResults());
            statuses.AddOnCompletedListener((result) => processResults());
            locks.AddOnCompletedListener((result) => processResults());

            return menuEncounters;
        }

        protected virtual void ProcessResults(WaitableTask<List<MenuEncounter>> result,
            WaitableTask<Dictionary<int, Dictionary<SaveType, EncounterMetadata>>> metadataGroups,
            WaitableTask<Dictionary<int, EncounterEditLock>> locks,
            WaitableTask<Dictionary<int, EncounterBasicStatus>> statuses)
        {
            if (result.IsCompleted() || !metadataGroups.IsCompleted() || !statuses.IsCompleted() || !locks.IsCompleted())
                return;

            if (metadataGroups.Result.Value == null)
            {
                result.SetError(null);
                return;
            }

            var menuEncounters = new List<MenuEncounter>();
            foreach (var metadataGroup in metadataGroups.Result.Value)
                menuEncounters.Add(GetMenuEncounter(metadataGroup, locks, statuses));
            result.SetResult(menuEncounters);
        }

        protected virtual MenuEncounter GetMenuEncounter(
            KeyValuePair<int, Dictionary<SaveType, EncounterMetadata>> metadataGroup,
            WaitableTask<Dictionary<int, EncounterEditLock>> locks,
            WaitableTask<Dictionary<int, EncounterBasicStatus>> statuses)
        {
            EncounterBasicStatus status = null;
            if (statuses.Result.Value?.ContainsKey(metadataGroup.Key) == true)
                status = statuses.Result.Value[metadataGroup.Key];
            var menuEncounter = new MenuEncounter(metadataGroup.Value, status);

            if (!locks.Result.IsError() && locks.Result.Value?.ContainsKey(metadataGroup.Key) == true)
                menuEncounter.Lock = locks.Result.Value[metadataGroup.Key];
            
            return menuEncounter;
        }
    }
}