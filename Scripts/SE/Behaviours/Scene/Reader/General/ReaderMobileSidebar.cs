using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSidebar : MonoBehaviour, ICloseHandler
    {
        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected ISidebarController SidebarController { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector, ISidebarController sidebarController)
        {
            EncounterSelector = encounterSelector;
            SidebarController = sidebarController;
        }
        protected virtual void Start()
        {
            EncounterSelector.Selected += OnEncounterSelected;
            if (EncounterSelector.CurrentValue != null)
                OnEncounterSelected(EncounterSelector, EncounterSelector.CurrentValue);
        }

        public virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs userEncounter)
            => StartCoroutine(CloseAfterSecond());

        protected IEnumerator CloseAfterSecond()
        {
            yield return new WaitForSeconds(2f);
            SidebarController.Close();
        }

        public void Close(object sender) => SidebarController.Close();
    }
}
