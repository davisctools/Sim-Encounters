using System;

namespace ClinicalTools.UI
{
    public class UrlTagsFormatter
    {
        protected VisitedLinksManager VisitedLinksManager { get; }
        public UrlTagsFormatter(VisitedLinksManager visitedLinksManager) => VisitedLinksManager = visitedLinksManager;

        public virtual string AppendUrlTag(string text, ParsedElement element, IChildTagsFormatter childTagsFormatter)
            => GetFormattedUrlOpeningTag(text, element) + childTagsFormatter.FormatChildren(element.Children)
                + FormattedUrlClosingTag;

        public virtual string GetFormattedUrlOpeningTag(string text, ParsedElement element)
        {
            var url = GetUrl(element);
            if (url == null)
                return element.ToString();

            return $"{text}<u><color={GetLinkColor(url)}><link=\"URL:{url}\">";
        }
        public string FormattedUrlClosingTag => "</link></color></u>";


        protected virtual string GetLinkColor(string url)
            => (VisitedLinksManager.IsLinkVisited(url)) ? "#551A8B" : "#0000EE";


        protected virtual string GetUrl(ParsedElement element)
        {
            var tag = element.StartTag;
            var index = tag.IndexOf("href", StringComparison.InvariantCultureIgnoreCase);
            return index >= 0 ? GetQuotedText(tag.Substring(index)) : null;
        }

        protected virtual string GetQuotedText(string text)
        {
            var indexSingleQuote = text.IndexOf('\'');
            var indexDoubleQuote = text.IndexOf('"');
            if (indexSingleQuote < 0)
                return GetQuotedText(text, '"', indexDoubleQuote);
            else if (indexDoubleQuote < 0 || indexSingleQuote < indexDoubleQuote)
                return GetQuotedText(text, '\'', indexSingleQuote);
            else
                return GetQuotedText(text, '"', indexDoubleQuote);
        }

        protected virtual string GetQuotedText(string text, char quoteChar, int firstIndex)
        {
            if (firstIndex < 0 || firstIndex + 1 == text.Length)
                return null;
            text = text.Substring(firstIndex + 1);

            var index = text.IndexOf(quoteChar);
            return index >= 0 ? text.Substring(0, index) : null;
        }
    }
}