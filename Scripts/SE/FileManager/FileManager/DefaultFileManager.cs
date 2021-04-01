using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class DefaultFileManager : IFileManager
    {
        protected string DefaultDirectory => Path.Combine(Application.streamingAssetsPath, "Default");
        protected string EncounterFilename => "encounter";

        private readonly IFileExtensionGetter fileExtensionManager;
        private readonly IServerStringReader serverReader;
        public DefaultFileManager(IFileExtensionGetter fileExtensionManager, IServerStringReader serverReader)
        {
            this.fileExtensionManager = fileExtensionManager;
            this.serverReader = serverReader;
        }

        public void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents)
            => throw new Exception("Cannot write to default files");

        public WaitableTask<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata)
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

        public WaitableTask<string[]> GetFilesText(User user, FileType fileType)
            => new WaitableTask<string[]>(new Exception());

        protected string GetFile(FileType fileType)
        {
            var path = Path.Combine(DefaultDirectory, EncounterFilename);
            var extension = fileExtensionManager.GetExtension(fileType);
            return $"{path}.{extension}";
        }

        public void UpdateFilename(User user, EncounterMetadata metadata)
            => throw new Exception("Cannot update names of default files");
        public void DeleteFiles(User user, EncounterMetadata metadata)
            => throw new Exception("Cannot delete default files");
    }
}