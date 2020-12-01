using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IUrlBuilder
    {
        string BuildUrl(string page, IEnumerable<UrlArgument> arguments = null);
    }
}