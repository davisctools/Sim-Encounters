using System;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public abstract class WriterMetadataDropdown : WriterMetadataBehaviour
    {
        private TMP_Dropdown dropdown;
        protected TMP_Dropdown Dropdown {
            get {
                if (dropdown == null)
                    dropdown = GetComponent<TMP_Dropdown>();
                return dropdown;
            }
        }

        protected virtual void SetValue(string value)
        {
            for (int i = 0; i < Dropdown.options.Count; i++) {
                if (!Dropdown.options[i].text.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                    continue;
                Dropdown.value = i;
                return;
            }
            Debug.LogWarning($"Could not find value ({value}) for encounter metadata dropdown.");
        }

        protected virtual string GetValue() => Dropdown.options[Dropdown.value].text;
    }
}