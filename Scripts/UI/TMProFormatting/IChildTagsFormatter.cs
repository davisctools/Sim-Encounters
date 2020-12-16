using System.Collections.Generic;

namespace ClinicalTools.UI
{
    public interface IChildTagsFormatter
    {
        string FormatChildren(IEnumerable<object> children);
    }
}