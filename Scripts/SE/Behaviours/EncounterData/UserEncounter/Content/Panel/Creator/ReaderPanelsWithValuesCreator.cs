﻿using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelsWithValuesCreator : ReaderGeneralPanelsCreator<BaseReaderPanelBehaviour>
    {
        public List<string> HasAllValues { get => hasAllValues; set => hasAllValues = value; }
        [SerializeField] private List<string> hasAllValues;

        protected override BaseReaderPanelBehaviour GetChildPanelPrefab(UserPanel childPanel)
        {
            foreach (var valueKey in HasAllValues) {
                if (childPanel.Data.LegacyValues.ContainsKey(valueKey))
                    continue;
                if (!childPanel.IsRead())
                    childPanel.SetChildPanelsRead(true);
                return null;
            }

            return base.GetChildPanelPrefab(childPanel);
        }

    }
}