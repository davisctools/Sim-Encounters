using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterGrantLabel : EncounterMetadataLabel
    {
        public GameObject[] ControlledObjects { get => controlledObjects; set => controlledObjects = value; }
        [SerializeField] private GameObject[] controlledObjects;

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
        {
            Label.text = eventArgs.Metadata.GrantInfo;
            if (ControlledObjects != null && string.IsNullOrWhiteSpace(eventArgs.Metadata.GrantInfo)) {
                foreach (var controlledObject in ControlledObjects)
                    controlledObject.SetActive(false);
            }
        }
    }
}