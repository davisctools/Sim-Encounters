using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class ListDeserializer<T> : IStringDeserializer<List<T>>
    {
        protected IStringSplitter StringSplitter { get; }
        protected IStringDeserializer<T> ElementParser { get; }
        public ListDeserializer(IStringDeserializer<T> elementParser, IStringSplitter stringSplitter)
        {
            ElementParser = elementParser;
            StringSplitter = stringSplitter;
        }

        public List<T> Deserialize(string text)
        {
            if (text == null)
                return null;
            var splitText = StringSplitter.Split(text);
            if (splitText == null)
                return null;

            var list = new List<T>();
            foreach (var textElement in splitText) {
                var element = ElementParser.Deserialize(textElement);
                if (element != null)
                    list.Add(element);
            }

            return list;
        }
    }
}