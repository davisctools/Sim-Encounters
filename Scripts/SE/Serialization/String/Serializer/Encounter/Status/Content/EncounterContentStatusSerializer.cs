namespace ClinicalTools.SimEncounters
{
    public class EncounterContentStatusSerializer : IStringSerializer<EncounterContentStatus>
    {
        private const char END_CHAR = ' ';
        private readonly IStatusSerializer<SectionStatus> sectionStatusSerializer;
        public EncounterContentStatusSerializer(IStatusSerializer<SectionStatus> sectionStatusSerializer)
        {
            this.sectionStatusSerializer = sectionStatusSerializer;
        }

        public string Serialize(EncounterContentStatus encounterStatus)
        {
            var str = encounterStatus.Read ? "1" : "0";

            foreach (var section in encounterStatus.SectionStatuses) {
                var sectionStr = sectionStatusSerializer.Serialize(section.Value, encounterStatus.Read);
                if (!string.IsNullOrWhiteSpace(sectionStr))
                    str += section.Key + sectionStr + END_CHAR;
            }

            return str.Trim();
        }
    }
}