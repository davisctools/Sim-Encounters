using SimpleJSON;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class XmlStringDeserializer<T> : IStringDeserializer<T>
    {
        private readonly IStringDeserializer<XmlDocument> xmlParser;
        private readonly IObjectSerializer<T> serializationFactory;
        public XmlStringDeserializer(IStringDeserializer<XmlDocument> xmlParser, IObjectSerializer<T> serializationFactory)
        {
            this.xmlParser = xmlParser;
            this.serializationFactory = serializationFactory;
        }

        public T Deserialize(string text)
        {
            var xmlDoc = xmlParser.Deserialize(text);
            var deserializer = new XmlDeserializer(xmlDoc);
            return serializationFactory.Deserialize(deserializer);
        }
    }
}