using System;
using System.Collections.Generic;

namespace ClinicalTools.UI
{
    public class TagsFormatter : IChildTagsFormatter
    {
        protected UrlTagsFormatter UrlTagsFormatter { get; }
        protected ListTagsFormatter ListTagsFormatter { get; set; }
        public TagsFormatter(UrlTagsFormatter urlTagsFormatter) => UrlTagsFormatter = urlTagsFormatter;

        public virtual string ProcessText(string text) => ProcessDocument(new ParsedDocument(text));
        public virtual string ProcessDocument(ParsedDocument document)
        {
            ListTagsFormatter = new ListTagsFormatter();
            return AppendChildren("", document.Children);
        }

        public virtual string FormatChildren(IEnumerable<object> children) => AppendChildren("", children);

        protected virtual string AppendChildren(string text, IEnumerable<object> children)
        {
            foreach (var child in children)
                text = AppendChild(text, child);
            return text;
        }

        protected virtual string AppendChild(string text, object child)
        {
            if (child is ParsedText textChild)
                return text + textChild.Text;
            else if (child is ParsedElement elementChild)
                return AppendElementNode(text, elementChild);
            else
                return text;
        }

        protected virtual string AppendElementNode(string text, ParsedElement element)
        {
            if (element.StartTag.Equals("<ul>", StringComparison.InvariantCultureIgnoreCase))
                return ListTagsFormatter.AppendUnorderedList(text, element, this);
            else if (element.StartTag.Equals("<ol>", StringComparison.InvariantCultureIgnoreCase))
                return ListTagsFormatter.AppendOrderedList(text, element, this);
            else if (element.StartTag.StartsWith("<a ", StringComparison.InvariantCultureIgnoreCase))
                return UrlTagsFormatter.AppendUrlTag(text, element, this);
            else if (element.StartTag.Equals("<noparse>", StringComparison.InvariantCultureIgnoreCase))
                return text + element.ToString(); 
            else
                return AppendGenericElementNode(text, element);
        }

        protected virtual string AppendGenericElementNode(string text, ParsedElement element)
            => $"{text}{element.StartTag}{AppendChildren("", element.Children)}{element.EndTag}";
    }
}