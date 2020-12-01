#if !STANDALONE_SCENE
using ImaginationOverflow.UniversalDeepLinking;
#endif

namespace ClinicalTools.SimEncounters
{
    public class QuickActionFactory
    {
#if !STANDALONE_SCENE
        private const string RecordNumberKey = "id";
        public virtual QuickAction GetLinkAction(LinkActivation linkAction)
        {
            if (!linkAction.QueryString.ContainsKey(RecordNumberKey))
                return new QuickAction();

            var recordNumberStr = linkAction.QueryString[RecordNumberKey];
            if (!int.TryParse(recordNumberStr, out int recordNumber))
                return new QuickAction();

            return new QuickAction(QuickActionType.Reader, recordNumber);
        }
#endif
    }
}