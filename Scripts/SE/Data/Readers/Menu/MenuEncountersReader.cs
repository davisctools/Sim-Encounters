using System.Collections.Generic;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class EncounterLocksReader : IEncounterLocksReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IStringDeserializer<List<EncounterEditLock>> parser;
        public EncounterLocksReader(
            IUrlBuilder urlBuilder, 
            IServerReader serverReader, 
            IStringDeserializer<List<EncounterEditLock>> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public virtual WaitableTask<Dictionary<int, EncounterEditLock>> GetEncounterLocks(User user)
        {
            var webRequest = GetWebRequest(user);
            var serverOutput = serverReader.Begin(webRequest);
            var metadatas = new WaitableTask<Dictionary<int, EncounterEditLock>>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(metadatas, result));

            return metadatas;
        }

        protected virtual string MenuPhp { get; } = "Lock.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "getLocks";
        protected virtual string AccountVariable { get; } = "accountId";

        protected virtual UnityWebRequest GetWebRequest(User user)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(AccountVariable, user.AccountId.ToString())
            };
            var url = urlBuilder.BuildUrl(MenuPhp, arguments);
            return UnityWebRequest.Get(url);
        }


        protected virtual void ProcessResults(
            WaitableTask<Dictionary<int, EncounterEditLock>> result, 
            TaskResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var locks = parser.Deserialize(serverOutput.Value);
            var lockDictionary = new Dictionary<int, EncounterEditLock>();
            foreach (var encounterLock in locks) {
                if (!lockDictionary.ContainsKey(encounterLock.RecordNumber))
                    lockDictionary.Add(encounterLock.RecordNumber, encounterLock);
            } 
            result.SetResult(lockDictionary);
        }
    }
    public interface IEncounterLocksReader
    {
        WaitableTask<Dictionary<int, EncounterEditLock>> GetEncounterLocks(User user);
    }

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