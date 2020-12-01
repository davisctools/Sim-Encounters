using System;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterDateModifiedLabel : EncounterMetadataLabel
    {
        public string Prefix { get => prefix; set => prefix = value; }
        [SerializeField] private string prefix;

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
        {
            var time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            time = time.AddSeconds(eventArgs.Metadata.DateModified);
            if (time > DateTime.UtcNow || time.Year < 2015) {
                Debug.LogError("Invalid time");
                Label.text = "";
                return;
            }

            var timeString = time.ToLocalTime().ToString("MMMM d, yyyy");
            Label.text = $"Last updated: {timeString}";
        }
    }
}