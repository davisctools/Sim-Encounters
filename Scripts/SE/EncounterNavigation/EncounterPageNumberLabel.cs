using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterPageNumberLabel : MonoBehaviour
    {
        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

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
        protected EncounterNonImageContent NonImageContent
            => CurrentTab.Encounter.Data.Content.NonImageContent;
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab == eventArgs.SelectedTab)
                return;
            CurrentTab = eventArgs.SelectedTab;

            if (TabCount == -1)
                TabCount = NonImageContent.GetTabCount();

            Label.text = $"Page: {NonImageContent.GetCurrentTabNumber()}/{TabCount}";
        }

        protected virtual void OnDestroy() => UserTabSelector.Selected -= OnTabSelected;
    }
}