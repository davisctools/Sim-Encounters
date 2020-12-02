﻿using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseMenuEncounterOverview : MonoBehaviour
    {
        public abstract void Select(object sender, MenuEncounterSelectedEventArgs eventArgs);
        public abstract void Hide();
    }
}