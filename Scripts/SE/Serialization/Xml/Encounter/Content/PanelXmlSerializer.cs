using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class PanelXmlSerializer : IXmlSerializer<Panel>
    {
        protected virtual PanelXmlSerializer ChildPanelFactory => this;

        protected virtual IXmlSerializer<PinGroup> PinsFactory { get; }
        public PanelXmlSerializer(IXmlSerializer<PinGroup> pinsFactory)
        {
            PinsFactory = pinsFactory;
        }


        protected virtual XmlNodeInfo TypeInfo { get; } = new XmlNodeInfo("type");
        protected virtual XmlNodeInfo PinsInfo { get; } = new XmlNodeInfo("pins");

        protected virtual XmlCollectionInfo DataInfo { get; } = new XmlCollectionInfo("values", "value");
        protected virtual XmlCollectionInfo ChildPanelsInfo { get; } = new XmlCollectionInfo("panels", "panel");


        public virtual bool ShouldSerialize(Panel value) => value != null;

        public void Serialize(XmlSerializer serializer, Panel value)
        {
            serializer.AddString(TypeInfo, value.Type);
            serializer.AddStringKeyValuePairs(DataInfo, value.Values);
            serializer.AddKeyValuePairs(ChildPanelsInfo, value.ChildPanels, ChildPanelFactory);
            serializer.AddValue(PinsInfo, value.Pins, PinsFactory);
        }

        public virtual Panel Deserialize(XmlDeserializer deserializer)
        {
            var panel = CreatePanel(deserializer);

            AddData(deserializer, panel);
            AddChildPanels(deserializer, panel);
            AddPins(deserializer, panel);

            return panel;
        }

        protected virtual string GetType(XmlDeserializer deserializer)
            => deserializer.GetString(TypeInfo);
        protected virtual Panel CreatePanel(XmlDeserializer deserializer)
        {
            var type = GetType(deserializer);

            return new Panel(type);
        }

        protected virtual List<KeyValuePair<string, string>> GetDataPairs(XmlDeserializer deserializer)
            => deserializer.GetStringKeyValuePairs(DataInfo);
        protected virtual void AddData(XmlDeserializer deserializer, Panel panel)
        {
            var dataPairs = GetDataPairs(deserializer);
            if (dataPairs != null) {
                {
                    foreach (var pair in dataPairs) {
                        if (panel.Values.ContainsKey(pair.Key))
                            Debug.LogWarning($"{panel.Type} panel has duplicate data key (Key:\"{pair.Key}\"; Value1:\"{panel.Values[pair.Key]}\"; Value2:\"{pair.Value}\")");
                        else
                            panel.Values.Add(pair);
                    }
                }
            }
        }

        protected virtual List<KeyValuePair<string, Panel>> GetChildPanels(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(ChildPanelsInfo, ChildPanelFactory);
        protected virtual void AddChildPanels(XmlDeserializer deserializer, Panel panel)
        {
            var childPanels = GetChildPanels(deserializer);
            if (childPanels != null) {
                foreach (var childPanel in childPanels)
                    panel.ChildPanels.Add(childPanel);
            }
        }


        protected virtual PinGroup GetPins(XmlDeserializer deserializer)
            => deserializer.GetValue(PinsInfo, PinsFactory);
        protected virtual void AddPins(XmlDeserializer deserializer, Panel panel)
            => panel.Pins = GetPins(deserializer);
    }
}