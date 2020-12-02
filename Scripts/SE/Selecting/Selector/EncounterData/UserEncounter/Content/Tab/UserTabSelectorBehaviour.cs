using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorBehaviour : MonoBehaviour,
        ISelector<UserTabSelectedEventArgs>,
        ISelectedListener<TabSelectedEventArgs>
    {
        protected UserTabSelectedEventArgs UserTabValue { get; set; }
        UserTabSelectedEventArgs ISelectedListener<UserTabSelectedEventArgs>.CurrentValue => UserTabValue;
        public event SelectedHandler<UserTabSelectedEventArgs> UserTabSelected;
        event SelectedHandler<UserTabSelectedEventArgs> ISelectedListener<UserTabSelectedEventArgs>.Selected {
            add => UserTabSelected += value;
            remove => UserTabSelected -= value;
        }

        protected TabSelectedEventArgs TabValue { get; set; }
        TabSelectedEventArgs ISelectedListener<TabSelectedEventArgs>.CurrentValue => TabValue;
        public event SelectedHandler<TabSelectedEventArgs> TabSelected;
        event SelectedHandler<TabSelectedEventArgs> ISelectedListener<TabSelectedEventArgs>.Selected {
            add => TabSelected += value;
            remove => TabSelected -= value;
        }

        public virtual void Select(object sender, UserTabSelectedEventArgs eventArgs)
        {
            UserTabValue = eventArgs;
            UserTabSelected?.Invoke(sender, UserTabValue);
            
            TabValue = new TabSelectedEventArgs(eventArgs.SelectedTab.Data);
            TabSelected?.Invoke(sender, TabValue);
        }

        public class Factory : PlaceholderFactory<string, UserTabSelectorBehaviour> { }
    }
}