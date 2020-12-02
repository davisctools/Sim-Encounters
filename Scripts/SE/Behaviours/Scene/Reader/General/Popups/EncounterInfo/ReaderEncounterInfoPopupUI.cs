namespace ClinicalTools.SimEncounters
{
    public class ReaderEncounterInfoPopupUI : BaseReaderEncounterInfoPopup
    {
        public override void ShowEncounterInfo(UserEncounter userEncounter) => gameObject.SetActive(true);
    }
}