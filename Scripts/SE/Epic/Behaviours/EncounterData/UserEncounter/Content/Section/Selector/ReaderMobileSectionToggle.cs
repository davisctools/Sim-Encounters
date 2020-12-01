using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionToggle : ReaderSectionToggle
    {
        public GameObject Line { get => line; set => line = value; }
        [SerializeField] private GameObject line;
        protected override void SetColor(bool isOn)
        {
            base.SetColor(isOn);
            Line.SetActive(!isOn);
        }

    }
}
