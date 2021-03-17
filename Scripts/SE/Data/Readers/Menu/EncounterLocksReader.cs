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
}