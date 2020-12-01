namespace ClinicalTools.SimEncounters
{
    public class SectionStatusDeserializer : ICharEnumeratorDeserializer<SectionStatus>
    {
        private const char END_CHAR = ' ';

        private readonly ICharEnumeratorDeserializer<TabStatus> tabStatusParser;
        private readonly ICharEnumeratorDeserializer<string> keyParser;
        public SectionStatusDeserializer(
            ICharEnumeratorDeserializer<TabStatus> tabStatusParser,
            ICharEnumeratorDeserializer<string> keyParser)
        {
            this.tabStatusParser = tabStatusParser;
            this.keyParser = keyParser;
        }

        public SectionStatus Deserialize(CharEnumerator enumerator)
        {
            var status = new SectionStatus();

            if (enumerator.IsDone)
                return status;

            status.Read = enumerator.Current == '1';

            while (enumerator.MoveNext() && enumerator.Current != END_CHAR) {
                var sectionKey = keyParser.Deserialize(enumerator);
                var tabStatus = tabStatusParser.Deserialize(enumerator);
                status.AddTabStatus(sectionKey, tabStatus);
            }

            return status;
        }
    }
}