using System;

namespace ClinicalTools.SimEncounters
{
    public class WebException : Exception
    {
        public ServerOutcome Outcome { get; }
        public WebException(ServerOutcome outcome) : base() => Outcome = outcome;
        public WebException(ServerOutcome outcome, string message) : base(message) => Outcome = outcome;
    }

    public enum ServerOutcome
    {
        Success, WebRequestNotDone, NetworkError, HttpError, DownloadNotDone, ParsingError
    }
}