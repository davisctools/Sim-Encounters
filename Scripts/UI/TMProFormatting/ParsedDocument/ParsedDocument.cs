using System;
using System.Collections.Generic;

namespace ClinicalTools.UI
{
    public class ParsedDocument
    {
        public List<object> Children { get; }
        public ParsedDocument(string str) => Children = GetChildren(str.GetEnumerator());

        public List<object> GetChildren(CharEnumerator charEnumerator)
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
        public override string ToString()
        {
            var str = "";
            foreach (var child in Children)
                str += child.ToString();
            return str;
        }
    }
}