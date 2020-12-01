using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class DemoFileManager : IFileManager
    {
        protected string DemoDirectory => Path.Combine(Application.streamingAssetsPath, "DemoCases");
        protected string EncountersListFilename => "list.txt";

        private readonly IFileExtensionGetter fileExtensionManager;
        private readonly IServerReader serverReader;
        public DemoFileManager(IFileExtensionGetter fileExtensionManager, IServerReader serverReader)
        {
            this.fileExtensionManager = fileExtensionManager;
            this.serverReader = serverReader;
        }

        public void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents)
            => throw new Exception("Cannot write to demo files");

        public WaitableTask<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata)
        {
            var fileText = new WaitableTask<string>();

            var filePath = GetFile(fileType, metadata.Filename);
            var webRequest = UnityWebRequest.Get(filePath);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => SetFileResult(result, fileText));
            return fileText;
        }

        protected virtual void SetFileResult(TaskResult<string> serverResult, WaitableTask<string> fileText)
        {
            if (serverResult.IsError())
                fileText.SetError(serverResult.Exception);
            else
                fileText.SetResult(serverResult.Value);
        }

        public WaitableTask<string[]> GetFilesText(User user, FileType fileType)
        {
            var filesText = new WaitableTask<string[]>();
            var demoEncounters = GetDemoEncounters();
            demoEncounters.AddOnCompletedListener((result) => ReadFiles(result, filesText, fileType));

            return filesText;
        }

        protected void ReadFiles(TaskResult<string[]> demoEncounters, WaitableTask<string[]> result, FileType fileType)
        {
            if (demoEncounters == null || !demoEncounters.HasValue() || demoEncounters.Value.Length == 0) {
                result.SetResult(new string[0]);
                return;
            }

            var serverResults = new WaitableTask<string>[demoEncounters.Value.Length];
            for (int i = 0; i < demoEncounters.Value.Length; i++) {
                var filePath = GetFile(fileType, demoEncounters.Value[i]);
                var webRequest = UnityWebRequest.Get(filePath);
                serverResults[i] = serverReader.Begin(webRequest);
                serverResults[i].AddOnCompletedListener((serverResult) => SetFilesResults(result, serverResults));
            }
        }

        protected void SetFilesResults(WaitableTask<string[]> result, WaitableTask<string>[] serverResults)
        {
            foreach (var serverResult in serverResults) {
                if (serverResult == null || !serverResult.IsCompleted())
                    return;
            }

            var filesTexts = new List<string>();
            foreach (var serverResult in serverResults) {
                if (!serverResult.Result.IsError())
                    filesTexts.Add(serverResult.Result.Value);
            }

            result.SetResult(filesTexts.ToArray());
        }

        protected string GetFile(FileType fileType, string filename)
        {
            var path = Path.Combine(DemoDirectory, filename);
            var extension = fileExtensionManager.GetExtension(fileType);
            return $"{path}.{extension}";
        }

        private WaitableTask<string[]> demoEncounters;
        protected WaitableTask<string[]> GetDemoEncounters()
        {
            if (demoEncounters != null)
                return demoEncounters;

            demoEncounters = new WaitableTask<string[]>();
            var demoEncountersPath = Path.Combine(DemoDirectory, EncountersListFilename);
            var webRequest = UnityWebRequest.Get(demoEncountersPath);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(SetEncounters);

            return demoEncounters;
        }

        protected void SetEncounters(TaskResult<string> serverResult)
        {
            if (demoEncounters == null || demoEncounters.IsCompleted() || serverResult.IsError()) {
                demoEncounters.SetError(new Exception("Could not get demo encounters from file."));
                return;
            }

            var splitChars = new char[] { '\n', '\r' };
            var encounters = serverResult.Value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            demoEncounters.SetResult(encounters);
        }

        public void UpdateFilename(User user, EncounterMetadata metadata)
            => throw new Exception("Cannot update names of demo files");
        public void DeleteFiles(User user, EncounterMetadata metadata)
            => throw new Exception("Cannot delete demo files");
    }
}