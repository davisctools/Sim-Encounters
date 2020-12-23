using System;
using System.Collections.Generic;

namespace ClinicalTools.UI
{
    public class ParsedElement
    {
        public string StartTag { get; }
        public string EndTag { get; }
        public List<object> Children { get; }
        public ParsedElement(CharEnumerator charEnumerator)
        {
            StartTag = GetTag(charEnumerator);
            if (StartTag.Equals("<hr>", StringComparison.InvariantCultureIgnoreCase))
                return;

            Children = GetChildren(charEnumerator);
            EndTag = GetTag(charEnumerator);
        }

        protected virtual List<object> GetChildren(CharEnumerator charEnumerator)
        {
            List<object> children = new List<object>();
            while (charEnumerator.MoveNext()) {
                if (charEnumerator.Current != '<')
                    children.Add(new ParsedText(charEnumerator));

                try {
                    if (charEnumerator.Current == '<' && charEnumerator.MoveNext() && charEnumerator.Current != '/')
                        children.Add(new ParsedElement(charEnumerator));
                    else
                        break;
                } catch (InvalidOperationException) {
                    break;
                }
            }
            return children;

        }

        protected virtual string GetTag(CharEnumerator charEnumerator)
        {
            string text = "<";
            do {
                text += charEnumerator.Current;
            } while (charEnumerator.MoveNext() && charEnumerator.Current != '>');
            return text + '>';
        }

        public override string ToString()
        {
            var str = StartTag;
            foreach (var child in Children)
                str += child.ToString();
            str += EndTag;
            return str;
        }
    }
}