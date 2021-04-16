using SimpleJSON;
using System.Collections.Generic;
using System.Linq;

namespace ClinicalTools.SimEncounters
{
    public class JsonSerializer : IDataSerializer
    {
        protected virtual JSONNode Node { get; }

        public const string DefaultBaseTagName = "data";
        public JsonSerializer(JSONNode node) => Node = node;


        protected virtual JSONNode AddObjectNode<T>(JSONNode parent, string name, T value, IObjectSerializer<T> serializationFactory)
        {
            var node = AddObjectNode(parent, name);
            var serializer = new JsonSerializer(node);
            serializationFactory.Serialize(serializer, value);
            return node;
        }
        protected virtual JSONNode AddObjectNode(JSONNode parent, string name)
        {
            var node = new JSONObject();
            parent.Add(name, node);
            return node;
        }
        protected virtual JSONNode AddArrayNode(JSONNode parent, string name)
        {
            var node = new JSONArray();
            parent.Add(name, node);
            return node;
        }

        public virtual void AddString(XmlNodeInfo nodeData, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            Node.Add(nodeData.Name, new JSONString(value));
        }

        public virtual void AddBool(XmlNodeInfo nodeData, bool value)
            => Node.Add(nodeData.Name, new JSONBool(value));
        public virtual void AddInt(XmlNodeInfo nodeData, int value)
            => Node.Add(nodeData.Name, new JSONNumber(value));
        public virtual void AddFloat(XmlNodeInfo nodeData, float value)
            => Node.Add(nodeData.Name, new JSONNumber(value));


        public virtual void AddValue<T>(XmlNodeInfo nodeData, T value, IObjectSerializer<T> serializationFactory)
        {
            if (!serializationFactory.ShouldSerialize(value))
                return;

            AddObjectNode(Node, nodeData.Name, value, serializationFactory);
        }

        public virtual void AddStringList(XmlCollectionInfo collectionInfo, IEnumerable<string> list)
        {
            if (list.Count() == 0)
                return;

            var listNode = AddArrayNode(Node, collectionInfo.CollectionNode.Name);
            foreach (var value in list.Where(value => !string.IsNullOrWhiteSpace(value)))
                listNode.Add(collectionInfo.ElementNode.Name, new JSONString(value));
        }

        public virtual void AddList<T>(
            XmlCollectionInfo collectionInfo, 
            IEnumerable<T> list,
            IObjectSerializer<T> serializationFactory)
        {
            if (list.Count() == 0)
                return;

            var listNode = AddArrayNode(Node, collectionInfo.CollectionNode.Name);
            foreach (var value in list.Where(value => serializationFactory.ShouldSerialize(value)))
                AddObjectNode(listNode, collectionInfo.ElementNode.Name, value, serializationFactory);
        }

        protected virtual string KeyAttributeName { get; } = "id";
        public virtual void AddStringKeyValuePairs(
            XmlCollectionInfo collectionInfo,
            IEnumerable<KeyValuePair<string, string>> list)
        {
            if (list.Count() == 0)
                return;

            var collectionNode = AddObjectNode(Node, collectionInfo.CollectionNode.Name);
            foreach (var pair in list.Where(pair => !string.IsNullOrWhiteSpace(pair.Value)))
                collectionNode.Add(pair.Key, new JSONString(pair.Value));
        }

        public virtual void AddKeyValuePairs<T>(
            XmlCollectionInfo collectionInfo,
            IEnumerable<KeyValuePair<string, T>> list,
            IObjectSerializer<T> serializationFactory)
        {
            if (list.Count() == 0)
                return;

            var collectionNode = AddObjectNode(Node, collectionInfo.CollectionNode.Name);
            foreach (var pair in list.Where(pair => serializationFactory.ShouldSerialize(pair.Value)))
                AddObjectNode(collectionNode, pair.Key, pair.Value, serializationFactory);
        }
    }
}