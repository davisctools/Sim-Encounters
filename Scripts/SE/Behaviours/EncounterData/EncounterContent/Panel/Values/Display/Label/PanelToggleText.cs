using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [Serializable]
    public class PanelToggleText
    {
        public string ValueName { get => valueName; set => valueName = value; }
        [SerializeField] private string valueName;

        public string Text { get => text; set => text = value; }
        [SerializeField] private string text;
    }
}