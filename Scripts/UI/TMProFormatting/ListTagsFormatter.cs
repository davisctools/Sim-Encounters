using System;
using System.Collections.Generic;

namespace ClinicalTools.UI
{
    public class ListTagsFormatter
    {
        protected int IndentLevel { get; set; }

        protected virtual string AppendClosePreviousListTags(string text)
        {
            text = RemoveLastNewLine(text);
            text += $"{IndentClosingTag}{MarginClosingTag}";
            return text;
        }

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
        protected string EndWithNewLine(string text)
            => RemoveLastNewLine(text) + '\n';


        protected virtual string ReopenPreviousListTags()
            => $"{GetMarginOpeningTag()}{GetIndentOpeningTag()}";

        protected Stack<double> IndentAmountStack = new Stack<double>();

        protected virtual string GetMarginOpeningTag() => $"<margin-left={IndentLevel}em>";
        protected virtual string MarginClosingTag => "</margin>";
        protected virtual string GetIndentOpeningTag() => $"<indent={IndentAmountStack.Peek()}em>";
        protected virtual string IndentClosingTag => "</indent>";

        protected virtual string AppendListStart(string text)
        {
            if (IndentLevel++ > 0)
                text = AppendClosePreviousListTags(text);
            if (text.Length > 0)
                text = EndWithNewLine(text);
            text += GetMarginOpeningTag();

            return text;
        }
        protected virtual string AppendListEnd(string text)
        {
            text += MarginClosingTag;
            if (--IndentLevel > 0)
                text += ReopenPreviousListTags();

            return text;
        }

        protected int UnorderedIndentLevel { get; set; }
        public virtual string AppendUnorderedList(string text, ParsedElement element, IChildTagsFormatter childTagsFormatter)
        {
            UnorderedIndentLevel++;
            text = AppendListStart(text);
            text = AppendUnorderedListChildren(text, element.Children, childTagsFormatter);
            text = AppendListEnd(text);
            UnorderedIndentLevel--;

            return text;
        }

        protected virtual string AppendUnorderedListChildren(string text, IEnumerable<object> children, IChildTagsFormatter childTagsFormatter)
        {
            IndentAmountStack.Push(1);

            var bulletChar = GetBulletChar();
            var childrenStr = "";
            foreach (var child in children) {
                if (child is ParsedElement elementChild && elementChild.StartTag.Equals("<li>", StringComparison.InvariantCultureIgnoreCase))
                    childrenStr += GetUnorderedListItemText(bulletChar, elementChild, childTagsFormatter);
            }
            text += childrenStr.Trim();

            IndentAmountStack.Pop();
            return text;
        }

        protected char BulletChar1 = '\u2022';
        protected char BulletChar2 = '\u25E6';
        protected char BulletChar3 = '\u25AA';
        protected virtual char GetBulletChar()
        {
            if (UnorderedIndentLevel == 1)
                return BulletChar1;
            else if (UnorderedIndentLevel == 2)
                return BulletChar2;
            else
                return BulletChar3;
        }
        protected virtual string GetUnorderedListItemText(char bulletChar, ParsedElement element, IChildTagsFormatter childTagsFormatter)
            => "\n" + bulletChar + GetIndentOpeningTag()
                + childTagsFormatter.FormatChildren(element.Children).Trim() + IndentClosingTag;


        protected int OrderedIndentLevel { get; set; }
        public virtual string AppendOrderedList(string text, ParsedElement element, IChildTagsFormatter childTagsFormatter)
        {
            OrderedIndentLevel++;
            text = AppendListStart(text);
            text = AppendOrderedListChildren(text, element.Children, childTagsFormatter);
            text = AppendListEnd(text);
            OrderedIndentLevel--;

            return text;
        }
        protected virtual string AppendOrderedListChildren(string text, IEnumerable<object> children, IChildTagsFormatter childTagsFormatter)
        {
            IndentAmountStack.Push(1.5);

            var childrenStr = "";
            int i = 1;
            foreach (var child in children) {
                if (child is ParsedElement elementChild && elementChild.StartTag.Equals("<li>", StringComparison.InvariantCultureIgnoreCase))
                    childrenStr += GetOrderedListItemText(i++, elementChild, childTagsFormatter);
            }
            text += childrenStr.Trim();

            IndentAmountStack.Pop();
            return text;
        }

        protected virtual string GetOrderedListItemText(int index, ParsedElement element, IChildTagsFormatter childTagsFormatter)
            => "\n" + GetOrderedListItemStart(index++) + GetIndentOpeningTag()
                + childTagsFormatter.FormatChildren(element.Children).Trim() + IndentClosingTag;

        protected virtual string GetOrderedListItemStart(int i)
        {
            string str = OrderedIndentLevel % 2 == 1 ? GetNumericalChar(i) : GetAlphabeticalChar(i);
            return $"{str}.";
        }
        protected virtual string GetNumericalChar(int i) => $"{i}";
        protected virtual string GetAlphabeticalChar(int i) => $"{(char)(0x60 + i)}";
    }
}