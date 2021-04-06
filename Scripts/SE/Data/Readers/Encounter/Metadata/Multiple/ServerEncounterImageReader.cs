using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterImageReader : IServerEncounterImageReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerTextureReader serverReader;
        public ServerEncounterImageReader(IUrlBuilder urlBuilder, IServerTextureReader serverReader)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
        }

        public WaitableTask GetTexture(User user, EncounterMetadata metadata, EncounterImage image)
        {
            if (metadata.Image?.Sprite != null && metadata.Image.Id == image.Id) {
                image.Sprite = metadata.Image.Sprite;
                return WaitableTask.CompletedTask;
            }

            var webRequest = GetWebRequest(user, metadata, image.FileName);
            var serverOutput = serverReader.Begin(webRequest);
            var task = new WaitableTask();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(task, image, result));

            return task;
        }

        protected virtual void ProcessResults(WaitableTask task, EncounterImage image, TaskResult<Texture2D> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                task.SetError(serverOutput.Exception);
                return;
            }

            var texture = serverOutput.Value;
            if (texture == null) {
                task.SetError(new Exception("Could not get texture."));
                return;
            }
            image.Sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));
            task.SetCompleted();
        }

        protected virtual string MenuPhp { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string AccountVariable { get; } = "account";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "image";
        protected virtual string FileVariable { get; } = "file";

        protected virtual UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata, string textureName)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(AccountVariable, user.AccountId.ToString()),
                new UrlArgument(EncounterVariable, metadata.RecordNumber.ToString()),
                new UrlArgument(ActionVariable, ActionValue),
                new UrlArgument(FileVariable, textureName)
            };
            var url = urlBuilder.BuildUrl(MenuPhp, arguments);
            return UnityWebRequestTexture.GetTexture(url);
        }
    }
}
