using System.Collections.Generic;

namespace ClinicalTools.UI
{
    public class VisitedLinksManager
    {
        public HashSet<string> VisitedLinks { get; } = new HashSet<string>();

        public virtual bool IsLinkVisited(string link) => VisitedLinks.Contains(link.ToUpperInvariant());
        public virtual void VisitLink(string link)
        {
            link = link.ToUpperInvariant();
            if (!VisitedLinks.Contains(link))
                VisitedLinks.Add(link);
        }
    }
}