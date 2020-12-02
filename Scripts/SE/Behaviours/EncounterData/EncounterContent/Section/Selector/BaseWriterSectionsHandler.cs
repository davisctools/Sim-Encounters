
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterSectionsHandler : MonoBehaviour, ISectionSelector
    {
        public abstract void Display(Encounter encounter);
        public abstract event SectionSelectedHandler SectionSelected;
        public abstract void SelectSection(Section section);
        public abstract event Action<Section> SectionEdited;
        public abstract event Action<Section> SectionDeleted;
    }
}