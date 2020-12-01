using System;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterSelectedEventArgs : EventArgs
    {
        public UserEncounter Encounter { get; }
        public UserEncounterSelectedEventArgs(UserEncounter encounter) => Encounter = encounter;
    }
}