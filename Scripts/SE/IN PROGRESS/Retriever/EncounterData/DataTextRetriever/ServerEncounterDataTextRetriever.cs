using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterDataTextRetriever : IEncounterDataTextRetriever
    {
        protected IServerStringReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        public ServerEncounterDataTextRetriever(IServerStringReader serverReader, IUrlBuilder urlBuilder)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
        }


        public virtual WaitableTask<string> GetDataText(User user, OldEncounterMetadata metadata)
            => ServerReader.Begin(GetWebRequest(user, metadata));

        protected virtual string DownloadPhp { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "data";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string AccountVariable { get; } = "account";
        protected virtual UnityWebRequest GetWebRequest(User user, OldEncounterMetadata metadata)
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