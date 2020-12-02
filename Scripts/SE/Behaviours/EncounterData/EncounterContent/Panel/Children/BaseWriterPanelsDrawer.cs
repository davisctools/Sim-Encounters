using ClinicalTools.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterPanelsDrawer : MonoBehaviour
    {
        public abstract void DrawChildPanels(OrderedCollection<Panel> childPanels);
        public abstract void DrawDefaultChildPanels();
        public abstract OrderedCollection<Panel> SerializeChildren();
    }
}