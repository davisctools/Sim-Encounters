using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class XmlSerializer : IDataSerializer
    {
        protected virtual XmlNode Node { get; }

        public const string DefaultBaseTagName = "data";
        public XmlSerializer(XmlDocument xmlDocument)
        {
            Node = xmlDocument.DocumentElement;
            if (Node != null)
                return;

            Node = xmlDocument.CreateElement(DefaultBaseTagName);
            xmlDocument.AppendChild(Node);
        }

        protected XmlSerializer(XmlNode node) => Node = node;

        private const bool NODE_NAME_FIRST_LETTER_CAPITALIZED = false;
        protected virtual XmlElement CreateElement(string name, XmlNode parent)
        {
            if (NODE_NAME_FIRST_LETTER_CAPITALIZED && !char.IsUpper(name[0]))
                Debug.LogWarning($"XML element name should be capitalized: {name}");
            if (!NODE_NAME_FIRST_LETTER_CAPITALIZED && char.IsUpper(name[0]))
                Debug.LogWarning($"XML element name should not be capitalized: {name}");

            var node = parent.OwnerDocument.CreateElement(name);
            parent.AppendChild(node);

            return node;
        }

        protected virtual XmlElement CreateElement(string name, string value, XmlNode parent)
        {
            var node = CreateElement(name, parent);
            node.InnerText = UnityWebRequest.EscapeURL(value);

            return node;
        }

        public virtual void AddString(XmlNodeInfo nodeData, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            if (nodeData == XmlNodeInfo.RootValue)
                Node.InnerText = value;
            else
                CreateElement(nodeData.Name, value, Node);
        }

        public virtual void AddBool(XmlNodeInfo nodeData, bool value)
            => CreateElement(nodeData.Name, value.ToString(), Node);
        public virtual void AddInt(XmlNodeInfo nodeData, int value)
            => CreateElement(nodeData.Name, value.ToString(), Node);

        // considered storing each value in their own tag, but individual panel fields must store their colors as a string
        // later the serialization and deserialization for colors to and from a string should be moved out so panels can access it
        public virtual void AddColor(XmlNodeInfo nodeData, Color value)
            => CreateElement(nodeData.Name, $"{value.r},{value.g},{value.b},{value.a}", Node);

        public virtual void AddValue<T>(XmlNodeInfo nodeData, T value, IObjectSerializer<T> serializationFactory)
        {
            if (!serializationFactory.ShouldSerialize(value))
                return;

            var element = CreateElement(nodeData.Name, Node);
            var serializer = new XmlSerializer(element);
            serializationFactory.Serialize(serializer, value);
        }

        public virtual void AddStringList(XmlCollectionInfo collectionInfo, IEnumerable<string> list)
        {
            if (list.Count() == 0)
                return;

            var listNode = CreateElement(collectionInfo.CollectionNode.Name, Node);
            foreach (var value in list.Where(value => !string.IsNullOrWhiteSpace(value)))
                CreateElement(collectionInfo.ElementNode.Name, value, listNode);
        }

        public virtual void AddList<T>(XmlCollectionInfo collectionInfo, IEnumerable<T> list,
            IObjectSerializer<T> serializationFactory)
        {
            if (list.Count() == 0)
                return;

            var listNode = CreateElement(collectionInfo.CollectionNode.Name, Node);

            foreach (var value in list.Where(value => serializationFactory.ShouldSerialize(value))) {
                var childNode = CreateElement(collectionInfo.ElementNode.Name, listNode);
                var serializer = new XmlSerializer(childNode);
                serializationFactory.Serialize(serializer, value);
            }
        }

        protected virtual string KeyAttributeName { get; } = "id";
        public virtual void AddStringKeyValuePairs(XmlCollectionInfo collectionInfo,
            IEnumerable<KeyValuePair<string, string>> list)
        {
            if (list.Count() == 0)
                return;

            var listNode = CreateElement(collectionInfo.CollectionNode.Name, Node);

            foreach (var pair in list.Where(pair => !string.IsNullOrWhiteSpace(pair.Value))) {
                var childNode = CreateElement(collectionInfo.ElementNode.Name, pair.Value, listNode);
                childNode.SetAttribute(KeyAttributeName, pair.Key);
            }
        }

        public virtual void AddKeyValuePairs<T>(XmlCollectionInfo collectionInfo,
            IEnumerable<KeyValuePair<string, T>> list, IObjectSerializer<T> serializationFactory)
        {
            if (list.Count() == 0)
                return;

            var listNode = CreateElement(collectionInfo.CollectionNode.Name, Node);
            foreach (var pair in list.Where(pair => serializationFactory.ShouldSerialize(pair.Value))) {
                var childNode = CreateElement(collectionInfo.ElementNode.Name, listNode);
                childNode.SetAttribute(KeyAttributeName, pair.Key);

                var serializer = new XmlSerializer(childNode);
                serializationFactory.Serialize(serializer, pair.Value);
            }
        }

        public void AddFloat(XmlNodeInfo nodeData, float value)
        {
        }
    }
}