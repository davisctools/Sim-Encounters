using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class NonImageContentXmlSerializer : IXmlSerializer<EncounterNonImageContent>
    {
        protected virtual IXmlSerializer<Section> SectionFactory { get; }

        public NonImageContentXmlSerializer(IXmlSerializer<Section> sectionFactory)
        {
            SectionFactory = sectionFactory;
        }

        protected virtual XmlCollectionInfo SectionsInfo { get; } = new XmlCollectionInfo("sections", "section");


        public virtual bool ShouldSerialize(EncounterNonImageContent value) => value != null;

        public void Serialize(XmlSerializer serializer, EncounterNonImageContent value)
        {
            serializer.AddKeyValuePairs(SectionsInfo, value.Sections, SectionFactory);
        }

        public EncounterNonImageContent Deserialize(XmlDeserializer deserializer)
        {
            var encounterData = CreateEncounterData(deserializer);

            AddSections(deserializer, encounterData);

            return encounterData;
        }

        protected virtual EncounterNonImageContent CreateEncounterData(XmlDeserializer deserializer)
        {
            return new EncounterNonImageContent();
        }

        protected virtual List<KeyValuePair<string, Section>> GetSections(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SectionsInfo, SectionFactory);
        protected virtual void AddSections(XmlDeserializer deserializer, EncounterNonImageContent encounterData)
        {
            var sectionPairs = GetSections(deserializer);
            if (sectionPairs == null)
                return;

            foreach (var sectionPair in sectionPairs)
                encounterData.Sections.Add(sectionPair);
        }
    }
}
