namespace ClinicalTools.SimEncounters
{
    public class EncounterContentStatusDeserializer : IStringDeserializer<EncounterContentStatus>, ICharEnumeratorDeserializer<EncounterContentStatus>
    {
        private readonly ICharEnumeratorDeserializer<SectionStatus> sectionStatusParser;
        private readonly ICharEnumeratorDeserializer<string> keyParser;
        public EncounterContentStatusDeserializer(ICharEnumeratorDeserializer<SectionStatus> sectionStatusParser,
            ICharEnumeratorDeserializer<string> keyParser)
        {
            this.sectionStatusParser = sectionStatusParser;
            this.keyParser = keyParser;
        }

        public EncounterContentStatus Deserialize(string text)
        {
            if (text == null)
                return new EncounterContentStatus();
            var enumerator = new CharEnumerator(text);
            return Deserialize(enumerator);
        }

        public EncounterContentStatus Deserialize(CharEnumerator enumerator)
        {
            var status = new EncounterContentStatus();

            if (!enumerator.MoveNext())
                return status;

            status.Read = enumerator.Current == '1';

            while (enumerator.MoveNext()) {
                var sectionKey = keyParser.Deserialize(enumerator);
                var sectionStatus = sectionStatusParser.Deserialize(enumerator);
                status.AddSectionStatus(sectionKey, sectionStatus);
            }

            return status;
        }
    }
}