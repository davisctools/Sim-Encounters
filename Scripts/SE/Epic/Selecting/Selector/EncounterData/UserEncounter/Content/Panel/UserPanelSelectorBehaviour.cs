using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserPanelSelectorBehaviour : MonoBehaviour,
        ISelector<UserPanelSelectedEventArgs>,
        ISelectedListener<PanelSelectedEventArgs>
    {
        public UserPanelSelectedEventArgs CurrentValue { get; protected set; }
        protected PanelSelectedEventArgs CurrentPanelValue { get; set; }
        PanelSelectedEventArgs ISelectedListener<PanelSelectedEventArgs>.CurrentValue => CurrentPanelValue;

        public event SelectedHandler<UserPanelSelectedEventArgs> Selected;
        public event SelectedHandler<PanelSelectedEventArgs> PanelSelected;
        event SelectedHandler<PanelSelectedEventArgs> ISelectedListener<PanelSelectedEventArgs>.Selected {
            add => PanelSelected += value;
            remove => PanelSelected -= value;
        }

        public virtual void Select(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            CurrentValue = eventArgs;
            Selected?.Invoke(sender, eventArgs);

            CurrentPanelValue = new PanelSelectedEventArgs(eventArgs.SelectedPanel.Data);
            PanelSelected?.Invoke(sender, CurrentPanelValue);
        }
    }
}