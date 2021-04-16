using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IDataSerializer
    {
        void AddBool(XmlNodeInfo nodeData, bool value);
        void AddInt(XmlNodeInfo nodeData, int value);
        void AddFloat(XmlNodeInfo nodeData, float value);
        void AddKeyValuePairs<T>(XmlCollectionInfo collectionInfo, IEnumerable<KeyValuePair<string, T>> list, IObjectSerializer<T> serializationFactory);
        void AddList<T>(XmlCollectionInfo collectionInfo, IEnumerable<T> list, IObjectSerializer<T> serializationFactory);
        void AddString(XmlNodeInfo nodeData, string value);
        void AddStringKeyValuePairs(XmlCollectionInfo collectionInfo, IEnumerable<KeyValuePair<string, string>> list);
        void AddStringList(XmlCollectionInfo collectionInfo, IEnumerable<string> list);
        void AddValue<T>(XmlNodeInfo nodeData, T value, IObjectSerializer<T> serializationFactory);
    }
}