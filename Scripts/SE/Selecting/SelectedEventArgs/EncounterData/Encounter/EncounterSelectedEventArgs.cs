using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterSelectedEventArgs : EventArgs
    {
        public ContentEncounter Encounter { get; }
        public EncounterSelectedEventArgs(ContentEncounter encounter) => Encounter = encounter;
    }
}