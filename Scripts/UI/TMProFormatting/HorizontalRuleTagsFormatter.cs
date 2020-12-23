namespace ClinicalTools.UI
{
    public class HorizontalRuleTagsFormatter
    {
        protected string RemoveLastNewLine(string text)
        {
            for (int i = text.Length - 1; i >= 0; i--) {
                if (text[i] == '\n')
                    return text.Substring(0, i);

                if (!char.IsWhiteSpace(text[i]))
                    break;
            }

            return text;
        }

        protected string EndWithNewLine(string text) => RemoveLastNewLine(text) + '\n';

        public virtual string AppendHorizontalRule(string text, ParsedElement element, IChildTagsFormatter childTagsFormatter)
        {
            if (text.Length > 0)
                text = EndWithNewLine(text);
            text += "<alpha=#b0><s><alpha=#00><align=flush> .</align></color></s></color>";

            return text;
        }
    }
}