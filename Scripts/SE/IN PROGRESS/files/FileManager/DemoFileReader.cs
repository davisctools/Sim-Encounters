using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class DemoFileReader : IEncounterFileReader
    {
        protected string DemoDirectory => Path.Combine(Application.streamingAssetsPath, "Demo", "Encounters");
        protected string EncountersListFilename => "list.txt";

        private readonly IFilenameInfo filenameInfo;
        private readonly IServerStringReader serverStringReader;
        private readonly IServerTextureReader serverTextureReader;
        public DemoFileReader(IFilenameInfo filenameInfo, IServerStringReader serverStringReader, IServerTextureReader serverTextureReader)
        {
            this.filenameInfo = filenameInfo;
            this.serverStringReader = serverStringReader;
            this.serverTextureReader = serverTextureReader;
        }

        public WaitableTask<string> ReadTextFile(User user, EncounterMetadata metadata, EncounterDataFileType fileType)
        {
            var task = new WaitableTask<string>();

            var filePath = GetFilepath(metadata, fileType);
            var webRequest = UnityWebRequest.Get(filePath);
            var serverResult = serverStringReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => SetFileResult(task, result));
            return task;
        }
        protected virtual void SetFileResult(WaitableTask<string> task, TaskResult<string> serverResult)
        {
            if (serverResult.IsError())
                task.SetError(serverResult.Exception);
            else
                task.SetResult(serverResult.Value);
        }


        public WaitableTask<string[]> ReadTextFiles(User user, EncounterDataFileType fileType)
        {
            var filesText = new WaitableTask<string[]>();
            var demoEncounters = GetDemoEncounters();
            demoEncounters.AddOnCompletedListener((result) => ReadFiles(result, filesText, fileType));

            return filesText;
        }

        protected void ReadFiles(TaskResult<string[]> demoEncounters, WaitableTask<string[]> result, EncounterDataFileType fileType)
        {
            if (demoEncounters == null || !demoEncounters.HasValue() || demoEncounters.Value.Length == 0) {
                result.SetResult(new string[0]);
                return;
            }

            var serverResults = new WaitableTask<string>[demoEncounters.Value.Length];
            for (int i = 0; i < demoEncounters.Value.Length; i++) {
                var filePath = GetFilepath(demoEncounters.Value[i], fileType);
                var webRequest = UnityWebRequest.Get(filePath);
                serverResults[i] = serverStringReader.Begin(webRequest);
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


        private WaitableTask<string[]> demoEncounters;
        protected WaitableTask<string[]> GetDemoEncounters()
        {
            if (demoEncounters != null)
                return demoEncounters;

            demoEncounters = new WaitableTask<string[]>();
            var demoEncountersPath = Path.Combine(DemoDirectory, EncountersListFilename);
            var webRequest = UnityWebRequest.Get(demoEncountersPath);
            var serverResult = serverStringReader.Begin(webRequest);
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

        public WaitableTask<Texture2D> ReadTextureFile(User user, EncounterMetadata metadata, string filename)
        {
            var task = new WaitableTask<Texture2D>();

            var filePath = Path.Combine(GetImagesFolder(metadata), filename);
            var webRequest = UnityWebRequest.Get(filePath);
            var serverResult = serverTextureReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => SetFileResult(task, result));
            return task;
        }
        protected virtual void SetFileResult(WaitableTask<Texture2D> task, TaskResult<Texture2D> serverResult)
        {
            if (serverResult.IsError())
                task.SetError(serverResult.Exception);
            else
                task.SetResult(serverResult.Value);
        }



        public string GetEncounterFolder(EncounterMetadata metadata)
            => Path.Combine(DemoDirectory, metadata.GetDesiredFilename());
        protected virtual string SaveFolder { get; set; } = "save";
        public string GetSaveFolder(EncounterMetadata metadata)
            => Path.Combine(GetEncounterFolder(metadata), SaveFolder);
        public string GetSaveFolder(string encounterDirectory)
            => Path.Combine(encounterDirectory, SaveFolder);
        protected virtual string ImagesFolder { get; set; } = "images";
        public string GetImagesFolder(EncounterMetadata metadata)
            => Path.Combine(GetSaveFolder(metadata), ImagesFolder);

        protected string GetFilepath(EncounterMetadata metadata, EncounterDataFileType fileType)
        {
            var folder = GetSaveFolder(metadata);
            var filename = filenameInfo.GetFilename(fileType);
            return Path.Combine(folder, filename);
        }
        protected string GetFilepath(string encounterFolder, EncounterDataFileType fileType)
        {
            var folder = GetSaveFolder(encounterFolder);
            var filename = filenameInfo.GetFilename(fileType);
            return Path.Combine(folder, filename);
        }
        protected string GetImageFilepath(EncounterMetadata metadata, EncounterImage image) => Path.Combine(GetImagesFolder(metadata), image.Filename);
    }
}