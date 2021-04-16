using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IDataDeserializer
    {
        bool GetBool(XmlNodeInfo valueFinder);
        Color GetColor(XmlNodeInfo valueFinder);
        int GetInt(XmlNodeInfo valueFinder);
        float GetFloat(XmlNodeInfo valueFinder);
        List<KeyValuePair<string, T>> GetKeyValuePairs<T>(XmlCollectionInfo collectionInfo, IObjectSerializer<T> serializationFactory);
        List<KeyValuePair<string, T>> GetKeyValuePairs<T>(XmlCollectionInfo collectionInfo, KeyValuePair<XmlNodeInfo, XmlNodeInfo> keyValuePairInfo, IObjectSerializer<T> serializationFactory);
        List<T> GetList<T>(XmlCollectionInfo collectionInfo, IObjectSerializer<T> serializationFactory);
        string GetString(XmlNodeInfo valueFinder);
        List<KeyValuePair<string, string>> GetStringKeyValuePairs(XmlCollectionInfo collectionInfo);
        List<KeyValuePair<string, string>> GetStringKeyValuePairs(XmlCollectionInfo collectionInfo, KeyValuePair<XmlNodeInfo, XmlNodeInfo> keyValuePairInfo);
        List<string> GetStringList(XmlCollectionInfo collectionInfo);
        T GetValue<T>(XmlNodeInfo valueFinder, IObjectSerializer<T> serializationFactory);
    }
}