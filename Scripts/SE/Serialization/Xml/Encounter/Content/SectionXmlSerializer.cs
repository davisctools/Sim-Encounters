using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SectionXmlSerializer : IObjectSerializer<Section>
    {
        protected virtual IObjectSerializer<Tab> TabFactory { get; }
        protected IObjectSerializer<Color> ColorSerializer { get; }
        public SectionXmlSerializer(IObjectSerializer<Tab> tabFactory, IObjectSerializer<Color> colorSerializer)
        {
            TabFactory = tabFactory; 
            ColorSerializer = colorSerializer;
        }

        protected virtual XmlNodeInfo NameInfo { get; set; } = new XmlNodeInfo("name");
        protected virtual XmlNodeInfo IconKeyInfo { get; } = new XmlNodeInfo("icon");
        protected virtual XmlNodeInfo ColorInfo { get; } = new XmlNodeInfo("color");
        protected virtual XmlNodeInfo ConditionsInfo { get; } = new XmlNodeInfo("conditions");
        protected virtual XmlCollectionInfo TabsInfo { get; } = new XmlCollectionInfo("tabs", "tab");


        public virtual bool ShouldSerialize(Section value) => value != null;

        public virtual void Serialize(IDataSerializer serializer, Section value)
        {
            serializer.AddString(NameInfo, value.Name);
            serializer.AddString(IconKeyInfo, value.IconKey);
            serializer.AddValue(ColorInfo, value.Color, ColorSerializer);
            serializer.AddKeyValuePairs(TabsInfo, value.Tabs, TabFactory);
        }

        public virtual Section Deserialize(IDataDeserializer deserializer)
        {
            var section = CreateSection(deserializer);
            SetTabs(deserializer, section);

            return section;
        }

        protected virtual string GetName(IDataDeserializer deserializer)
            => deserializer.GetString(NameInfo);
        protected virtual string GetIconKey(IDataDeserializer deserializer)
            => deserializer.GetString(IconKeyInfo);
        protected virtual Color GetColor(IDataDeserializer deserializer)
            => deserializer.GetColor(ColorInfo);
        protected virtual Section CreateSection(IDataDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var iconKey = GetIconKey(deserializer);
            var color = GetColor(deserializer);

            return new Section(name, iconKey, color);
        }

        protected virtual List<KeyValuePair<string, Tab>> GetTabs(IDataDeserializer deserializer)
            => deserializer.GetKeyValuePairs(TabsInfo, TabFactory);
        protected virtual void SetTabs(IDataDeserializer deserializer, Section section)
        {
            var tabs = GetTabs(deserializer);
            if (tabs == null)
                return;

            foreach (var tab in tabs)
                section.Tabs.Add(tab);
        }
    }
}