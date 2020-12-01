using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class XmlDeserializer
    {
        protected virtual XmlNode Node { get; }

        public XmlDeserializer(XmlDocument xmlDocument) => Node = xmlDocument.DocumentElement;
        protected XmlDeserializer(XmlNode xmlNode) => Node = xmlNode;

        public virtual string GetName() => Node.Name;

        public virtual string GetString(XmlNodeInfo valueFinder)
        {
            var value = valueFinder.FindNodeText(Node);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value;
        }

        public virtual bool GetBool(XmlNodeInfo valueFinder)
        {
            var boolStr = GetString(valueFinder);
            if (boolStr == null)
                return false;

            boolStr = boolStr.Trim();

            if (bool.TryParse(boolStr, out var value))
                return value;
            else
                return false;
        }

        public virtual int GetInt(XmlNodeInfo valueFinder)
        {
            var intStr = GetString(valueFinder);
            if (intStr == null)
                return -1;

            intStr = intStr.Trim();

            if (int.TryParse(intStr, out var value))
                return value;
            else
                return -1;
        }

        public virtual Color GetColor(XmlNodeInfo valueFinder)
        {
            var colorStr = GetString(valueFinder);
            if (colorStr == null)
                return Color.clear;

            colorStr = colorStr.Trim();

            var colorParts = colorStr.Split(',');
            if (colorParts.Length != 4)
                return Color.clear;

            if (colorParts.Length == 4 && float.TryParse(colorParts[0], out var red) && float.TryParse(colorParts[1], out var green)
                && float.TryParse(colorParts[2], out var blue) && float.TryParse(colorParts[3], out var alpha)) {

                return new Color(red, green, blue, alpha);
            } else {
                return Color.clear;
            }
        }



        public virtual T GetValue<T>(XmlNodeInfo valueFinder, IXmlSerializer<T> serializationFactory)
            => GetValue(Node, valueFinder, serializationFactory);

        protected virtual T GetValue<T>(XmlNode node, XmlNodeInfo valueFinder, IXmlSerializer<T> serializationFactory)
        {
            if (node == null)
                return default;

            var valueNode = valueFinder.FindNode(node);
            if (valueNode == null)
                return default;

            return GetValue(valueNode, serializationFactory);
        }
        protected virtual T GetValue<T>(XmlNode valueNode, IXmlSerializer<T> serializationFactory)
        {
            var deserializer = new XmlDeserializer(valueNode);

            try {
                return serializationFactory.Deserialize(deserializer);
            } catch (Exception ex) {
                Debug.LogWarning($"{ex.Message}\n{ex.StackTrace}");
                //Debug.Log(ex.StackTrace);
                return default;
            }
        }

        protected virtual IEnumerable<XmlNode> GetElementNodes(XmlCollectionInfo collectionInfo)
        {
            var valueNode = collectionInfo.CollectionNode.FindNode(Node);
            if (valueNode == null)
                return null;

            return collectionInfo.ElementNode.GetNodeList(valueNode);
        }

        public virtual List<string> GetStringList(XmlCollectionInfo collectionInfo)
        {
            var nodes = GetElementNodes(collectionInfo);
            if (nodes == null)
                return null;

            var list = new List<string>();
            foreach (var node in nodes) {
                var value = collectionInfo.ElementNode.GetText(node);
                if (value != null)
                    list.Add(value);
            }

            return list;
        }

        public virtual List<T> GetList<T>(XmlCollectionInfo collectionInfo, IXmlSerializer<T> serializationFactory)
        {
            var nodes = GetElementNodes(collectionInfo);
            if (nodes == null)
                return null;

            var list = new List<T>();
            foreach (var node in nodes) {
                var value = GetValue(node, serializationFactory);
                if (value != null)
                    list.Add(value);
            }

            return list;
        }

        public virtual List<KeyValuePair<string, string>> GetStringKeyValuePairs(XmlCollectionInfo collectionInfo)
            => GetStringKeyValuePairs(collectionInfo, DefaultKeyValuePairInfo);
        public virtual List<KeyValuePair<string, string>> GetStringKeyValuePairs(
            XmlCollectionInfo collectionInfo, KeyValuePair<XmlNodeInfo, XmlNodeInfo> keyValuePairInfo)
        {
            var elementNodes = GetElementNodes(collectionInfo);
            if (elementNodes == null)
                return null;

            var pairs = new List<KeyValuePair<string, string>>();
            foreach (var node in elementNodes) {
                var key = keyValuePairInfo.Key.FindNodeText(node);
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                var value = keyValuePairInfo.Value.FindNodeText(node);
                if (value != null)
                    pairs.Add(new KeyValuePair<string, string>(key, value));
            }

            return pairs;
        }

        protected virtual KeyValuePair<XmlNodeInfo, XmlNodeInfo> DefaultKeyValuePairInfo { get; } =
            new KeyValuePair<XmlNodeInfo, XmlNodeInfo>(
                new XmlNodeInfo("id", XmlTagComparison.AttributeNameEquals),
                XmlNodeInfo.RootValue
            );
        public virtual List<KeyValuePair<string, T>> GetKeyValuePairs<T>(XmlCollectionInfo collectionInfo, IXmlSerializer<T> serializationFactory)
            => GetKeyValuePairs(collectionInfo, DefaultKeyValuePairInfo, serializationFactory);
        public virtual List<KeyValuePair<string, T>> GetKeyValuePairs<T>(
            XmlCollectionInfo collectionInfo, KeyValuePair<XmlNodeInfo, XmlNodeInfo> keyValuePairInfo, IXmlSerializer<T> serializationFactory)
        {
            var elementNodes = GetElementNodes(collectionInfo);
            if (elementNodes == null)
                return null;

            var pairs = new List<KeyValuePair<string, T>>();
            foreach (var node in elementNodes) {
                var key = keyValuePairInfo.Key.FindNodeText(node);
                var value = GetValue(node, keyValuePairInfo.Value, serializationFactory);
                if (value == null)
                    continue;

                pairs.Add(new KeyValuePair<string, T>(key, value));
            }

            return pairs;
        }

    }
}