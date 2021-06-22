using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class AudienceFilterItem : EncounterFilterItem
    {
        public Toggle Toggle { get; set; }
        public string AudienceCompareText { get; set; }
        public AudienceFilterItem(Toggle toggle, string audienceDisplay, string audienceCompareText) : base(audienceDisplay)
        {
            Toggle = toggle;
            AudienceCompareText = audienceCompareText;
        }
    }
}