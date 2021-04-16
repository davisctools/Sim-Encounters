using SimpleJSON;

namespace ClinicalTools.SimEncounters
{
    public class JsonStringDeserializer<T> : IStringDeserializer<T>
    {
        private readonly IObjectSerializer<T> serializationFactory;
        public JsonStringDeserializer(IObjectSerializer<T> serializationFactory)
        {
            this.serializationFactory = serializationFactory;
        }

        public T Deserialize(string text)
        {
            var node = JSON.Parse(text);
            var deserializer = new JsonDeserializer(node);
            return serializationFactory.Deserialize(deserializer);
        }
    }
}