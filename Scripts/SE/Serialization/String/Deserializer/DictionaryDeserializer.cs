using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class DictionaryDeserializer<TKey, TValue> : IStringDeserializer<Dictionary<TKey, TValue>>
    {
        protected IStringSplitter StringSplitter { get; }
        protected IStringDeserializer<KeyValuePair<TKey, TValue>> PairParser { get; }

        public DictionaryDeserializer(IStringDeserializer<KeyValuePair<TKey, TValue>> elementParser, IStringSplitter stringSplitter)
        {
            PairParser = elementParser;
            StringSplitter = stringSplitter;
        }

        public Dictionary<TKey, TValue> Deserialize(string text)
        {
            if (text == null)
                return null;
            var splitText = StringSplitter.Split(text);
            if (splitText == null)
                return null;

            var dict = new Dictionary<TKey, TValue>();
            foreach (var textElement in splitText) {
                var pair = PairParser.Deserialize(textElement);
                if (pair.Value != null && !dict.ContainsKey(pair.Key))
                    dict.Add(pair.Key, pair.Value);
            }

            return dict;
        }
    }
}