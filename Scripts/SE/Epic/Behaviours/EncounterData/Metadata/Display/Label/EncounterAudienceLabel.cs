﻿using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterAudienceLabel : EncounterMetadataLabel
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => Label.text = eventArgs.Metadata.Audience;
    }
}