using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class JsonDeserializer : IDataDeserializer
    {
        protected virtual JSONNode Node { get; }

        public JsonDeserializer(JSONNode xmlNode) => Node = xmlNode;

        public virtual string GetString(XmlNodeInfo valueFinder)
        {
            var node = Node[valueFinder.Name];
            return !node.IsNull && node.IsString ? node : null;
        }
        public virtual bool GetBool(XmlNodeInfo valueFinder)
        {
            var node = Node[valueFinder.Name];
            return !node.IsNull && node.IsBoolean && (bool)node;
        }

        public virtual int GetInt(XmlNodeInfo valueFinder)
        {
            var node = Node[valueFinder.Name];
            return !node.IsNull && node.IsNumber ? (int)node : -1;
        }
        public virtual float GetFloat(XmlNodeInfo valueFinder)
        {
            var node = Node[valueFinder.Name];
            return !node.IsNull && node.IsNumber ? (float)node : -1;
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

        public virtual T GetValue<T>(XmlNodeInfo valueFinder, IObjectSerializer<T> serializationFactory)
            => GetValue(Node, valueFinder, serializationFactory);

        protected virtual T GetValue<T>(JSONNode node, XmlNodeInfo valueFinder, IObjectSerializer<T> serializationFactory)
        {
            if (node == null)
                return default;

            var valueNode = node[valueFinder.Name];
            return GetValue(valueNode, serializationFactory);
        }
        protected virtual T GetValue<T>(JSONNode valueNode, IObjectSerializer<T> serializationFactory)
        {
            var deserializer = new JsonDeserializer(valueNode);

            try {
                return serializationFactory.Deserialize(deserializer);
            } catch (Exception ex) {
                Debug.LogWarning($"{ex.Message}\n{ex.StackTrace}");
                //Debug.Log(ex.StackTrace);
                return default;
            }
        }

        protected virtual IEnumerable<JSONNode> GetListElementNodes(XmlCollectionInfo collectionInfo)
        {
            var valueNode = Node[collectionInfo.CollectionNode.Name];
            return !valueNode.IsNull ? valueNode.Children : null;
        }

        protected virtual JSONNode.Enumerator GetKeyedCollectionElementNodes(
            XmlCollectionInfo collectionInfo)
        {
            var valueNode = Node[collectionInfo.CollectionNode.Name];
            return (!valueNode.IsObject) ? valueNode.GetEnumerator() : new JSONNode.Enumerator();
        }

        public virtual List<string> GetStringList(XmlCollectionInfo collectionInfo)
        {
            var nodes = GetListElementNodes(collectionInfo);
            if (nodes == null)
                return null;

            var list = new List<string>();
            foreach (var node in nodes) {
                if (!node.IsNull)
                    list.Add(node);
            }

            return list;
        }

        public virtual List<T> GetList<T>(XmlCollectionInfo collectionInfo, IObjectSerializer<T> serializationFactory)
        {
            var nodes = GetListElementNodes(collectionInfo);
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
            var nodeEnumerator = GetKeyedCollectionElementNodes(collectionInfo);
            if (!nodeEnumerator.IsValid)
                return null;

            var pairs = new List<KeyValuePair<string, string>>();
            do {
                var value = nodeEnumerator.Current.Value;
                if (!value.IsNull)
                    pairs.Add(new KeyValuePair<string, string>(nodeEnumerator.Current.Key, value));
            } while (nodeEnumerator.MoveNext());

            return pairs;
        }

        protected virtual KeyValuePair<XmlNodeInfo, XmlNodeInfo> DefaultKeyValuePairInfo { get; } =
            new KeyValuePair<XmlNodeInfo, XmlNodeInfo>(
                new XmlNodeInfo("id", XmlTagComparison.AttributeNameEquals),
                XmlNodeInfo.RootValue
            );
        public virtual List<KeyValuePair<string, T>> GetKeyValuePairs<T>(XmlCollectionInfo collectionInfo, IObjectSerializer<T> serializationFactory)
            => GetKeyValuePairs(collectionInfo, DefaultKeyValuePairInfo, serializationFactory);
        public virtual List<KeyValuePair<string, T>> GetKeyValuePairs<T>(
            XmlCollectionInfo collectionInfo, KeyValuePair<XmlNodeInfo, XmlNodeInfo> keyValuePairInfo, IObjectSerializer<T> serializationFactory)
        {
            var nodeEnumerator = GetKeyedCollectionElementNodes(collectionInfo);
            if (!nodeEnumerator.IsValid)
                return null;

            var pairs = new List<KeyValuePair<string, T>>();
            do {
                var valueNode = nodeEnumerator.Current.Value;
                if (valueNode.IsNull || !valueNode.IsObject)
                    continue;

                var value = GetValue(valueNode, keyValuePairInfo.Value, serializationFactory);
                pairs.Add(new KeyValuePair<string, T>(nodeEnumerator.Current.Key, value));
            } while (nodeEnumerator.MoveNext());

            return pairs;
        }
    }
}