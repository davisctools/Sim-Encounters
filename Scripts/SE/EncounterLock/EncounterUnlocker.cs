﻿using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class EncounterUnlocker : IEncounterUnlocker
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerStringReader serverReader;
        private readonly IStringDeserializer<EncounterEditLock> parser;
        public EncounterUnlocker(IUrlBuilder urlBuilder, IServerStringReader serverReader, IStringDeserializer<EncounterEditLock> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public virtual WaitableTask UnlockEncounter(User user, OldEncounterMetadata metadata)
        {
            var webRequest = GetWebRequest(user, metadata);
            var serverOutput = serverReader.Begin(webRequest);
            var task = new WaitableTask();
            serverOutput.AddOnCompletedListener((result) => OnServerResult(task, result));

            return task;
        }

        protected virtual string Php { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "lock";
        protected virtual string SubactionVariable { get; } = "subaction";
        protected virtual string SubactionValue { get; } = "unlock";
        protected virtual string AccountVariable { get; } = "account";
        protected virtual string UsernameVariable { get; } = "editor";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual UnityWebRequest GetWebRequest(User user, OldEncounterMetadata metadata)
        {
            var url = urlBuilder.BuildUrl(Php);
            var form = CreateForm(user, metadata);
            return UnityWebRequest.Post(url, form);
        }

        protected virtual WWWForm CreateForm(User user, OldEncounterMetadata metadata)
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

        protected virtual void OnServerResult(WaitableTask result, TaskResult<string> serverOutput)
        {
            try {
                ProcessResults(serverOutput);
                result.SetCompleted();
            } catch (Exception ex) {
                result.SetError(ex);
            }
        }
        protected virtual void ProcessResults(TaskResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError())
                throw serverOutput.Exception;

            var output = serverOutput.Value.Trim();
            if (output.StartsWith("1"))
                return;

            // TODO: use server errors
            string errorStart = "-1|";
            if (output.StartsWith(errorStart, StringComparison.InvariantCultureIgnoreCase)) {
                var encounterLock = parser.Deserialize(output.Substring(errorStart.Length));
                throw new EncounterAlreadyLockedException(encounterLock);
            }

            throw new Exception("Could not unlock the encounter.");
        }
    }
}
