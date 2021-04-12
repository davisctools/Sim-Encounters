using System;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class WriterDialogueDirectionDropdown : WriterDropdownPanelField
    {
        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.LegacyValues;
            if (values.ContainsKey("characterName"))
                Dropdown.value = values["characterName"].Equals("Provider", StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;
            else
                base.OnPanelSelected(sender, e);
        }
    }
}