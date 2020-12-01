using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterPanelCreator : MonoBehaviour
    {

        public abstract event Action<BaseWriterAddablePanel> AddPanel;

        public abstract void Initialize(List<BaseWriterAddablePanel> options);
    }
}