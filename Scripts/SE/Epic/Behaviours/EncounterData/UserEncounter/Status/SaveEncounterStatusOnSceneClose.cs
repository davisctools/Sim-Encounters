﻿using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SaveEncounterStatusOnSceneClose : MonoBehaviour
    {
        protected IStatusWriter StatusWriter { get; set; }
        protected ISelector<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        [Inject]
        public virtual void Inject(
            IStatusWriter statusWriter,
            ISelector<UserEncounterSelectedEventArgs> encounterSelector)
        {
            EncounterSelector = encounterSelector;
            StatusWriter = statusWriter;
        }
        protected virtual void OnDestroy() => SaveStatus();
        protected void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveStatus();
        }

        protected virtual void SaveStatus()
        {
            var encounter = EncounterSelector.CurrentValue.Encounter;
            if (encounter == null)
                return;

            var status = encounter.Status;
            status.BasicStatus.Completed = status.ContentStatus.Read;
            StatusWriter.WriteStatus(encounter);
        }
    }
}