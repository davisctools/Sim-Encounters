using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterPageNumberLabel : MonoBehaviour
    {
        protected TextMeshProUGUI Label => (label == null) ? label = GetComponent<TextMeshProUGUI>() : label;
        private TextMeshProUGUI label;

        protected ISelectedListener<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserTabSelectedEventArgs> userTabSelector)
        {
            UserTabSelector = userTabSelector;
        }

        protected virtual void Start()
        {
            UserTabSelector.Selected += OnTabSelected;
            if (UserTabSelector.CurrentValue != null)
                OnTabSelected(UserTabSelector, UserTabSelector.CurrentValue);
        }

        protected int TabCount { get; set; } = -1;
        protected UserTab CurrentTab { get; set; }
        protected EncounterContent Content => CurrentTab.Encounter.Data.Content;
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab == eventArgs.SelectedTab)
                return;
            CurrentTab = eventArgs.SelectedTab;

            if (TabCount == -1)
                TabCount = Content.GetTabCount();

            Label.text = $"Page: {Content.GetCurrentTabNumber()}/{TabCount}";
        }

        protected virtual void OnDestroy() => UserTabSelector.Selected -= OnTabSelected;
    }
}