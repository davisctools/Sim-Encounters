using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseMenuEncounterMetadataSelector : MonoBehaviour
    {
        public abstract WaitableTask<KeyValuePair<SaveType, EncounterMetadata>> GetMetadata(MenuEncounter menuEncounter); 
    }
}