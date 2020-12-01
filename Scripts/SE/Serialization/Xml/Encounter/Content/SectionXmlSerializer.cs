using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SectionXmlSerializer : IXmlSerializer<Section>
    {
        protected virtual IXmlSerializer<Tab> TabFactory { get; }
        public SectionXmlSerializer(IXmlSerializer<Tab> tabFactory)
        {
            TabFactory = tabFactory;
        }

        protected virtual XmlNodeInfo NameInfo { get; set; } = new XmlNodeInfo("name");
        protected virtual XmlNodeInfo IconKeyInfo { get; } = new XmlNodeInfo("icon");
        protected virtual XmlNodeInfo ColorInfo { get; } = new XmlNodeInfo("color");
        protected virtual XmlNodeInfo ConditionsInfo { get; } = new XmlNodeInfo("conditions");
        protected virtual XmlCollectionInfo TabsInfo { get; } = new XmlCollectionInfo("tabs", "tab");


        public virtual bool ShouldSerialize(Section value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, Section value)
        {
            serializer.AddString(NameInfo, value.Name);
            serializer.AddString(IconKeyInfo, value.IconKey);
            serializer.AddColor(ColorInfo, value.Color);
            serializer.AddKeyValuePairs(TabsInfo, value.Tabs, TabFactory);
        }

        public virtual Section Deserialize(XmlDeserializer deserializer)
        {
            var section = CreateSection(deserializer);

            SetTabs(deserializer, section);

            return section;
        }

        protected virtual string GetName(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
        protected virtual string GetIconKey(XmlDeserializer deserializer)
            => deserializer.GetString(IconKeyInfo);
        protected virtual Color GetColor(XmlDeserializer deserializer)
            => deserializer.GetColor(ColorInfo);
        protected virtual Section CreateSection(XmlDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var iconKey = GetIconKey(deserializer);
            var color = GetColor(deserializer);

            return new Section(name, iconKey, color);
        }

        protected virtual List<KeyValuePair<string, Tab>> GetTabs(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(TabsInfo, TabFactory);
        protected virtual void SetTabs(XmlDeserializer deserializer, Section section)
        {
            var tabs = GetTabs(deserializer);
            if (tabs == null)
                return;

            foreach (var tab in tabs)
                section.Tabs.Add(tab);
        }
    }
}