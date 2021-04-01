using System;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterLocker : IEncounterLocker
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerStringReader serverReader;
        private readonly IStringDeserializer<EncounterEditLock> parser;
        public EncounterLocker(SignalBus signalBus, IUrlBuilder urlBuilder, IServerStringReader serverReader, IStringDeserializer<EncounterEditLock> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public virtual WaitableTask LockEncounter(User user, EncounterMetadata metadata)
        {
            var webRequest = GetWebRequest(user, metadata);
            var serverOutput = serverReader.Begin(webRequest);
            var task = new WaitableTask();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(task, result));

            return task;
        }

        protected virtual string Php { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "mode";
        protected virtual string ActionValue { get; } = "lock";
        protected virtual string SubactionVariable { get; } = "subaction";
        protected virtual string SubactionValue { get; } = "lock";
        protected virtual string AccountVariable { get; } = "account";
        protected virtual string UsernameVariable { get; } = "editor";
        protected virtual string EncounterVariable { get; } = "encounter";

        protected virtual UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var url = urlBuilder.BuildUrl(Php);
            var form = CreateForm(user, metadata);
            return UnityWebRequest.Post(url, form);
        }


        protected virtual WWWForm CreateForm(User user, EncounterMetadata metadata)
        {
            var form = new WWWForm();

            form.AddField(ModeVariable, ModeValue);
            form.AddField(ActionVariable, ActionValue);
            form.AddField(SubactionVariable, SubactionValue);
            form.AddField(AccountVariable, user.AccountId.ToString());
            form.AddField(UsernameVariable, Environment.UserName);
            form.AddField(EncounterVariable, metadata.RecordNumber.ToString());

            return form;
        }


        protected virtual void ProcessResults(WaitableTask result, TaskResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var output = serverOutput.Value.Trim();
            if (string.IsNullOrWhiteSpace(output) || output.StartsWith("1")) {
                result.SetCompleted();
                return;
            }

            string errorStart = "error|";
            if (output.StartsWith(errorStart, StringComparison.InvariantCultureIgnoreCase)) {
                var encounterLock = parser.Deserialize(output.Substring(errorStart.Length));
                result.SetError(new EncounterAlreadyLockedException(encounterLock));
                return;
            }

            result.SetError(new Exception("Could not set encounter lock."));
        }
    }
}
