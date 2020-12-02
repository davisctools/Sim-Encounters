using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [Serializable]
    public class OptionWriterPanel
    {
        public BaseWriterPanel PanelPrefab { get => panelPrefab; set => panelPrefab = value; }
        [SerializeField] private BaseWriterPanel panelPrefab;
    }
}