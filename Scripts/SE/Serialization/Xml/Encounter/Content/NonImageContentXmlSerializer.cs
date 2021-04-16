using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class NonImageContentXmlSerializer : IObjectSerializer<EncounterContentData>
    {
        protected virtual IObjectSerializer<Section> SectionFactory { get; }
        protected virtual IObjectSerializer<Character> CharacterFactory { get; }

        public NonImageContentXmlSerializer(IObjectSerializer<Section> sectionFactory, IObjectSerializer<Character> characterFactory)
        {
            SectionFactory = sectionFactory;
            CharacterFactory = characterFactory;
        }

        protected virtual XmlCollectionInfo CharactersInfo { get; } = new XmlCollectionInfo("characters", "character");
        protected virtual XmlCollectionInfo SectionsInfo { get; } = new XmlCollectionInfo("sections", "section");


        public virtual bool ShouldSerialize(EncounterContentData value) => value != null;

        public void Serialize(IDataSerializer serializer, EncounterContentData value)
        {
            serializer.AddKeyValuePairs(CharactersInfo, value.Characters, CharacterFactory);
            serializer.AddKeyValuePairs(SectionsInfo, value.Sections, SectionFactory);
        }

        public EncounterContentData Deserialize(IDataDeserializer deserializer)
        {
            var encounterData = CreateEncounterData(deserializer);

            AddCharacters(deserializer, encounterData);
            AddSections(deserializer, encounterData);

            return encounterData;
        }

        protected virtual EncounterContentData CreateEncounterData(IDataDeserializer deserializer)
        {
            return new EncounterContentData();
        }

        protected virtual List<KeyValuePair<string, Character>> GetCharacters(IDataDeserializer deserializer)
            => deserializer.GetKeyValuePairs(CharactersInfo, CharacterFactory);
        protected virtual void AddCharacters(IDataDeserializer deserializer, EncounterContentData encounterData)
        {
            var characterPairs = GetCharacters(deserializer);
            if (characterPairs == null)
                return;

            foreach (var characterPair in characterPairs)
                encounterData.Characters.Add(characterPair);
        }

        protected virtual List<KeyValuePair<string, Section>> GetSections(IDataDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SectionsInfo, SectionFactory);
        protected virtual void AddSections(IDataDeserializer deserializer, EncounterContentData encounterData)
        {
            var sectionPairs = GetSections(deserializer);
            if (sectionPairs == null)
                return;

            foreach (var sectionPair in sectionPairs)
                encounterData.Sections.Add(sectionPair);
        }
    }
}
