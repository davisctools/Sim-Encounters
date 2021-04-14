using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class DefaultFileReader : IEncounterFileReader
    {
        protected string DefaultDirectory => Path.Combine(Application.streamingAssetsPath, "Default");
        protected string EncounterFolder => "encounter";

        private readonly IFilenameInfo filenameInfo;

        private readonly IServerStringReader serverReader;
        public DefaultFileReader(IFilenameInfo filenameInfo, IServerStringReader serverReader)
        {
            this.filenameInfo = filenameInfo;
            this.serverReader = serverReader;
        }

        public WaitableTask<string> ReadTextFile(User user, OldEncounterMetadata metadata, EncounterDataFileType fileType)
        {
            var fileText = new WaitableTask<string>();

            var filePath = GetFile(fileType);
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

        public WaitableTask<string[]> ReadTextFiles(User user, EncounterDataFileType fileType) => new WaitableTask<string[]>(new Exception());

        protected string GetFile(EncounterDataFileType fileType)
            => Path.Combine(GetEncounterFolder(), filenameInfo.GetFilename(fileType));
        protected string GetEncounterFolder() => Path.Combine(DefaultDirectory, EncounterFolder);

        public WaitableTask<Texture2D> ReadTextureFile(User user, OldEncounterMetadata metadata, string filename)
            => new WaitableTask<Texture2D>(new NotImplementedException());
    }
}