using System;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class EncounterUnlocker : IEncounterUnlocker
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IStringDeserializer<EncounterEditLock> parser;
        public EncounterUnlocker(IUrlBuilder urlBuilder, IServerReader serverReader, IStringDeserializer<EncounterEditLock> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public virtual WaitableTask UnlockEncounter(User user, EncounterMetadata metadata)
        {
            var webRequest = GetWebRequest(user, metadata);
            var serverOutput = serverReader.Begin(webRequest);
            var task = new WaitableTask();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(task, result));

            return task;
        }

        protected virtual string LockPhp { get; } = "Lock.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "unlock";
        protected virtual string AccountVariable { get; } = "accountId";
        protected virtual string UsernameVariable { get; } = "username";
        protected virtual string RecordNumberVariable { get; } = "recordNumber";
        protected virtual UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(AccountVariable, user.AccountId.ToString()),
                new UrlArgument(UsernameVariable, Environment.UserName),
                new UrlArgument(RecordNumberVariable, metadata.RecordNumber.ToString())
            };

            var url = urlBuilder.BuildUrl(LockPhp, arguments);
            return UnityWebRequest.Get(url);
        }

        protected virtual void ProcessResults(WaitableTask result, TaskResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var output = serverOutput.Value.Trim();
            if (output.StartsWith("1")) {
                result.SetCompleted();
                return;
            }

            string errorStart = "-1|";
            if (output.StartsWith(errorStart, StringComparison.InvariantCultureIgnoreCase)) {
                var encounterLock = parser.Deserialize(output.Substring(errorStart.Length));
                result.SetError(new EncounterAlreadyLockedException(encounterLock));
                return;
            }

            result.SetError(new Exception("Could not unlock the encounter."));
        }
    }
}
