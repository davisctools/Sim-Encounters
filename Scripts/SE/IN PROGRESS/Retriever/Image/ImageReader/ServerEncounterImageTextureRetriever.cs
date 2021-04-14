using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterImageTextureRetriever : IEncounterImageTextureRetriever
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerTextureReader serverReader;
        public ServerEncounterImageTextureRetriever(IUrlBuilder urlBuilder, IServerTextureReader serverReader)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
        }

        public WaitableTask<Texture2D> GetTexture(User user, OldEncounterMetadata metadata, EncounterImage image)
            => serverReader.Begin(GetWebRequest(user, metadata, image.Filename));

        protected virtual string MenuPhp { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string AccountVariable { get; } = "account";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "image";
        protected virtual string FileVariable { get; } = "file";

        protected virtual UnityWebRequest GetWebRequest(User user, OldEncounterMetadata metadata, string textureName)
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
