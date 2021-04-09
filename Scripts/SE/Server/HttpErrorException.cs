namespace ClinicalTools.SimEncounters
{
    public class HttpErrorException : WebException
    {
        public long ResponseCode { get; }
        public string ServerResponse { get; }
        public HttpErrorException(string message, long responseCode, string serverResponse) : base(ServerOutcome.HttpError, message)
        {
            ResponseCode = responseCode;
            ServerResponse = serverResponse;
        }
    }
}