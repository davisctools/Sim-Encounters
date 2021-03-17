using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ShowOnEncounterLocked : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private BaseTooltip tooltip;
        [SerializeField] private TextMeshProUGUI tooltipText;

        protected virtual SignalBus SignalBus { get; set; }
        protected virtual ISelectedListener<MenuEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            SignalBus signalBus,
            ISelectedListener<MenuEncounterSelectedEventArgs> encounterSelectedListener)
        {
            SignalBus = signalBus;
            EncounterSelectedListener = encounterSelectedListener;
        }

        protected virtual void Awake()
        {
            EncounterSelectedListener.Selected += EncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                EncounterSelected(this, EncounterSelectedListener.CurrentValue);
            SignalBus.Subscribe<EncounterLocksUpdatedSignal>(UpdateEncounterLocks);
        }

        protected virtual void OnDestroy()
        {
            if (EncounterSelectedListener != null)
                EncounterSelectedListener.Selected -= EncounterSelected;
            if (SignalBus != null)
                SignalBus.Unsubscribe<EncounterLocksUpdatedSignal>(UpdateEncounterLocks);
        }

        protected virtual void UpdateEncounterLocks()
            => gameObject.SetActive(EncounterSelectedListener.CurrentValue.Encounter.Lock != null);
        protected virtual void EncounterSelected(object sender, MenuEncounterSelectedEventArgs e)
            => gameObject.SetActive(e.Encounter.Lock != null);

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            var encounterLock = EncounterSelectedListener.CurrentValue.Encounter.Lock;
            if (encounterLock == null)
                return;

            DateTimeOffset time2 = DateTimeOffset.FromUnixTimeSeconds(encounterLock.StartEditTime);
            DateTime time = time2.LocalDateTime;
            tooltipText.text = $"Encounter being edited by {encounterLock.EditorName},\n" +
                $"since {time:MMMM d, h:mm tt}";
            tooltip.Show();
        }

        public void OnPointerExit(PointerEventData eventData) => tooltip.Hide();
    }
}
