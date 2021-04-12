using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterImagesJsonRetriever : IEncounterImagesJsonRetriever
    {
        protected IServerStringReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        public ServerEncounterImagesJsonRetriever(IServerStringReader serverReader, IUrlBuilder urlBuilder)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
        }
        public WaitableTask<string> GetImagesJson(User user, EncounterMetadata metadata)
        {
            var webRequest = GetWebRequest(user, metadata);
            return ServerReader.Begin(webRequest);
        }

        protected virtual string DownloadPhp { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "images";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string AccountVariable { get; } = "account";

        protected virtual UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(AccountVariable, user.AccountId.ToString()),
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(ActionVariable, ActionValue),
                new UrlArgument(EncounterVariable, metadata.RecordNumber.ToString()),
            };
            var url = UrlBuilder.BuildUrl(DownloadPhp, arguments);
            return UnityWebRequest.Get(url);
        }
    }
}