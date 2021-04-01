using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerImageContentReader : IImageContentReader
    {
        private readonly IServerStringReader serverReader;
        private readonly IUrlBuilder urlBuilder;
        private readonly IStringDeserializer<LegacyEncounterImageContent> parser;
        public ServerImageContentReader(IServerStringReader serverReader, IUrlBuilder urlBuilder, IStringDeserializer<LegacyEncounterImageContent> parser)
        {
            this.serverReader = serverReader;
            this.urlBuilder = urlBuilder;
            this.parser = parser;
        }

        public WaitableTask<LegacyEncounterImageContent> GetImageData(User user, EncounterMetadata metadata)
        {
            var imageData = new WaitableTask<LegacyEncounterImageContent>();

            var webRequest = GetWebRequest(user, metadata);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => ProcessResults(imageData, result));

            return imageData;
        }

        protected virtual string DownloadPhp { get; } = "DownloadEncounters.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "downloadCase";
        protected virtual string RecordNumberVariable { get; } = "recordNumber";
        protected virtual string ColumnVariable { get; } = "column";
        protected virtual string ColumnValue { get; } = "imgData";
        protected virtual string AccountIdVariable { get; } = "accountId";
        private UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(AccountIdVariable, user.AccountId.ToString()),
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(RecordNumberVariable, metadata.RecordNumber.ToString()),
                new UrlArgument(ColumnVariable, ColumnValue),
            };
            var url = urlBuilder.BuildUrl(DownloadPhp, arguments);
            return UnityWebRequest.Get(url);
        }

        private void ProcessResults(WaitableTask<LegacyEncounterImageContent> result, TaskResult<string> serverResult)
        {
            result.SetResult(parser.Deserialize(serverResult.Value));
        }
    }
}