namespace ClinicalTools.SimEncounters
{
    public interface ISectionSelector
    {
        event SectionSelectedHandler SectionSelected;
        void SelectSection(Section section);
    }
}