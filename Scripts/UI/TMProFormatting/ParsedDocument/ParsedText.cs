using System;

namespace ClinicalTools.UI
{
    public class ParsedText
    {
        public string Text { get; }

        public ParsedText(CharEnumerator charEnumerator)
            => Text = GetText(charEnumerator);

        protected virtual string GetText(CharEnumerator charEnumerator)
        {
            string text = "";
            do {
                text += charEnumerator.Current;
            } while (charEnumerator.MoveNext() && charEnumerator.Current != '<');
            return text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}