namespace ClinicalTools.SimEncounters
{
    public class SectionStatusSerializer : IStatusSerializer<SectionStatus>
    {
        private const char END_CHAR = ' ';
        private readonly IStatusSerializer<TabStatus> tabStatusSerializer;
        public SectionStatusSerializer(IStatusSerializer<TabStatus> tabStatusSerializer)
        {
            this.tabStatusSerializer = tabStatusSerializer;
        }

        public string Serialize(SectionStatus status, bool parentRead)
        {
            var str = "";
            foreach (var tab in status.TabStatuses) {
                var tabStr = tabStatusSerializer.Serialize(tab.Value, status.Read);
                if (!string.IsNullOrWhiteSpace(tabStr))
                    str += tab.Key + tabStr + END_CHAR;
            }

            if (str.Length == 0 && status.Read == parentRead)
                return null;

            var readChar = status.Read ? '1' : '0';
            str = readChar + str;

            return str;
        }
    }
}