using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class TabXmlSerializer : IXmlSerializer<Tab>
    {
        protected virtual IXmlSerializer<Panel> PanelFactory { get; }
        // shared between sections, tabs, and panels

        public TabXmlSerializer(IXmlSerializer<Panel> panelFactory)
        {
            PanelFactory = panelFactory;
        }

        protected virtual XmlNodeInfo TypeInfo { get; } = new XmlNodeInfo("type");
        protected virtual XmlNodeInfo NameInfo { get; } = new XmlNodeInfo("name");
        protected virtual XmlNodeInfo ConditionsInfo { get; } = new XmlNodeInfo("conditions");

        protected virtual XmlCollectionInfo PanelsInfo { get; } = new XmlCollectionInfo("panels", "panel");

        public virtual bool ShouldSerialize(Tab value) => value != null;

        public void Serialize(XmlSerializer serializer, Tab value)
        {
            serializer.AddString(TypeInfo, value.Type);
            serializer.AddString(NameInfo, value.Name);
            serializer.AddKeyValuePairs(PanelsInfo, value.Panels, PanelFactory);
        }

        public virtual Tab Deserialize(XmlDeserializer deserializer)
        {
            var tab = CreateTab(deserializer);

            AddPanels(deserializer, tab);

            return tab;
        }

        protected virtual string GetType(XmlDeserializer deserializer)
            => deserializer.GetString(TypeInfo);
        protected virtual string GetName(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
        protected virtual Tab CreateTab(XmlDeserializer deserializer)
        {
            var type = GetType(deserializer);
            var name = GetName(deserializer);

            return new Tab(type, name);
        }

        protected virtual List<KeyValuePair<string, Panel>> GetPanels(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(PanelsInfo, PanelFactory);
        protected virtual void AddPanels(XmlDeserializer deserializer, Tab tab)
        {
            var panels = GetPanels(deserializer);
            if (panels == null)
                return;

            foreach (var panel in panels)
                tab.Panels.Add(panel);
        }
    }
}