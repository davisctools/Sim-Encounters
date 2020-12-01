using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class UrlBuilder : IUrlBuilder
    {
#if LOCALHOST 
        protected virtual string ServerAddress { get; } = @"http://localhost/SimEncounters/";
#else
        protected virtual string ServerAddress { get; } = @"https://takecontrolgame.com/docs/games/CECreator/PHP/";
#endif
        public virtual string BuildUrl(string page, IEnumerable<UrlArgument> arguments)
            => $"{ServerAddress}{page}{GetArgumentsString(arguments)}";

        protected virtual string GetArgumentsString(IEnumerable<UrlArgument> arguments)
        {
            if (arguments == null)
                return "";

            var argumentsString = "?";
            foreach (var argument in arguments)
                argumentsString += UrlArgument(argument);

            return argumentsString;
        }

        protected string UrlArgument(UrlArgument argument) => $"&{argument.Name}={argument.Value}";
    }
}