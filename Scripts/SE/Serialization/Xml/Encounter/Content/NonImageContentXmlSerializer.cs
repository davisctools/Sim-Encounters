using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class NonImageContentXmlSerializer : IXmlSerializer<EncounterContentData>
    {
        protected virtual IXmlSerializer<Section> SectionFactory { get; }
        protected virtual IXmlSerializer<Character> CharacterFactory { get; }

        public NonImageContentXmlSerializer(IXmlSerializer<Section> sectionFactory, IXmlSerializer<Character> characterFactory)
        {
            SectionFactory = sectionFactory;
            CharacterFactory = characterFactory;
        }

        protected virtual XmlCollectionInfo CharactersInfo { get; } = new XmlCollectionInfo("characters", "character");
        protected virtual XmlCollectionInfo SectionsInfo { get; } = new XmlCollectionInfo("sections", "section");


        public virtual bool ShouldSerialize(EncounterContentData value) => value != null;

        public void Serialize(XmlSerializer serializer, EncounterContentData value)
        {
            serializer.AddKeyValuePairs(CharactersInfo, value.Characters, CharacterFactory);
            serializer.AddKeyValuePairs(SectionsInfo, value.Sections, SectionFactory);
        }

        public EncounterContentData Deserialize(XmlDeserializer deserializer)
        {
            var encounterData = CreateEncounterData(deserializer);

            AddCharacters(deserializer, encounterData);
            AddSections(deserializer, encounterData);

            return encounterData;
        }

        protected virtual EncounterContentData CreateEncounterData(XmlDeserializer deserializer)
        {
            return new EncounterContentData();
        }

        protected virtual List<KeyValuePair<string, Character>> GetCharacters(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(CharactersInfo, CharacterFactory);
        protected virtual void AddCharacters(XmlDeserializer deserializer, EncounterContentData encounterData)
        {
            var characterPairs = GetCharacters(deserializer);
            if (characterPairs == null)
                return;

            foreach (var characterPair in characterPairs)
                encounterData.Characters.Add(characterPair);
        }

        protected virtual List<KeyValuePair<string, Section>> GetSections(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SectionsInfo, SectionFactory);
        protected virtual void AddSections(XmlDeserializer deserializer, EncounterContentData encounterData)
        {
            var sectionPairs = GetSections(deserializer);
            if (sectionPairs == null)
                return;

            foreach (var sectionPair in sectionPairs)
                encounterData.Sections.Add(sectionPair);
        }
    }
}
