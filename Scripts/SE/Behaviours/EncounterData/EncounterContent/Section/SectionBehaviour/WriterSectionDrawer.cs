
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class WriterSectionDrawer : BaseSectionDrawer
    {
        public virtual List<Image> SectionBorders { get => sectionBorders; set => sectionBorders = value; }
        [SerializeField] private List<Image> sectionBorders;

        public override void Display(ContentEncounter encounter, Section section)
        {
            var color = section.Color;
            foreach (var sectionBorder in SectionBorders)
                sectionBorder.color = color;
        }
    }
}