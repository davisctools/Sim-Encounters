using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelsWithValuesCreator : ReaderGeneralPanelsCreator<BaseReaderPanelBehaviour>
    {
        public List<string> HasAllValues { get => hasAllValues; set => hasAllValues = value; }
        [SerializeField] private List<string> hasAllValues;

        protected override BaseReaderPanelBehaviour GetChildPanelPrefab(UserPanel childPanel)
        {
            var childData = childPanel.Data;
            if ((childData.Values == null || childData.Values.Count == 0) && (childData.Pins == null || !childData.Pins.HasPin())) {
                if (!childPanel.IsRead())
                    childPanel.SetChildPanelsRead(true);
                return null;
            }

            foreach (var valueKey in HasAllValues) {
                if (childPanel.Data.Values.ContainsKey(valueKey))
                    continue;
                if (!childPanel.IsRead())
                    childPanel.SetChildPanelsRead(true);
                return null;
            }

            return base.GetChildPanelPrefab(childPanel);
        }

    }
}