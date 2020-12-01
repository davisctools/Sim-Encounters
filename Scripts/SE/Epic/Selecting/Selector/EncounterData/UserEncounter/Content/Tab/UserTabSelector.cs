namespace ClinicalTools.SimEncounters
{
    public class UserTabSelector : Selector<UserTabSelectedEventArgs>
    {
        protected ISelector<TabSelectedEventArgs> TabSelector { get; }
        public UserTabSelector(ISelector<TabSelectedEventArgs> tabSelector)
            => TabSelector = tabSelector;

        public override void Select(object sender, UserTabSelectedEventArgs value)
        {
            base.Select(sender, value);
            TabSelector.Select(this, new TabSelectedEventArgs(value.SelectedTab.Data));
        }
    }
}