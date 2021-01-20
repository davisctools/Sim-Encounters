using System;

namespace ClinicalTools.SimEncounters
{
    public enum EncounterSelectionType { Read, Edit }

    public class MenuEncounterSelectedEventArgs : EventArgs
    {
        public EncounterSelectionType SelectionType { get; }
        public MenuEncounter Encounter { get; }
        public MenuEncounterSelectedEventArgs(MenuEncounter encounter, EncounterSelectionType selectionType)
        {
            Encounter = encounter;
            SelectionType = selectionType;
        }
    }
}