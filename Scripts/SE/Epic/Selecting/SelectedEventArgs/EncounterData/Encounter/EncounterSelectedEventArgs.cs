using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterSelectedEventArgs : EventArgs
    {
        public Encounter Encounter { get; }
        public EncounterSelectedEventArgs(Encounter encounter) => Encounter = encounter;
    }
}