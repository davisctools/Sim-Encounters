using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class TabXmlSerializer : IObjectSerializer<Tab>
    {
        protected virtual IObjectSerializer<Panel> PanelFactory { get; }
        // shared between sections, tabs, and panels

        public TabXmlSerializer(IObjectSerializer<Panel> panelFactory)
        {
            PanelFactory = panelFactory;
        }

        protected virtual XmlNodeInfo TypeInfo { get; } = new XmlNodeInfo("type");
        protected virtual XmlNodeInfo NameInfo { get; } = new XmlNodeInfo("name");
        protected virtual XmlNodeInfo ConditionsInfo { get; } = new XmlNodeInfo("conditions");

        protected virtual XmlCollectionInfo PanelsInfo { get; } = new XmlCollectionInfo("panels", "panel");

        public virtual bool ShouldSerialize(Tab value) => value != null;

        public void Serialize(IDataSerializer serializer, Tab value)
        {
            serializer.AddString(TypeInfo, value.Type);
            serializer.AddString(NameInfo, value.Name);
            serializer.AddKeyValuePairs(PanelsInfo, value.Panels, PanelFactory);
        }

        public virtual Tab Deserialize(IDataDeserializer deserializer)
        {
            var tab = CreateTab(deserializer);

            AddPanels(deserializer, tab);

            return tab;
        }

        protected virtual string GetType(IDataDeserializer deserializer)
            => deserializer.GetString(TypeInfo);
        protected virtual string GetName(IDataDeserializer deserializer)
            => deserializer.GetString(NameInfo);
        protected virtual Tab CreateTab(IDataDeserializer deserializer)
        {
            var type = GetType(deserializer);
            var name = GetName(deserializer);

            return new Tab(type, name);
        }

        protected virtual List<KeyValuePair<string, Panel>> GetPanels(IDataDeserializer deserializer)
            => deserializer.GetKeyValuePairs(PanelsInfo, PanelFactory);
        protected virtual void AddPanels(IDataDeserializer deserializer, Tab tab)
        {
            var panels = GetPanels(deserializer);
            if (panels == null)
                return;

            foreach (var panel in panels)
                tab.Panels.Add(panel);
        }
    }
}