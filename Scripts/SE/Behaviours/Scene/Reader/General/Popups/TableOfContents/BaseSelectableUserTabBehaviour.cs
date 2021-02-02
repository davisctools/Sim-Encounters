using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseSelectableUserTabBehaviour : UserTabSelectorBehaviour
    {
        protected virtual ISelector<UserTabSelectedEventArgs> TabSelector { get; set; }
        protected virtual UserTab Tab { get; set; }

        [Inject]
        public virtual void Inject(ISelector<UserTabSelectedEventArgs> tabSelector)
        {
            if (tabSelector is BaseSelectableUserTabBehaviour s && s == this)
                throw new Exception("Injected Section Selector is itself. " +
                    $"Are you binding it correctly with {typeof(UserTabSelectorInstaller).Name}?");

            TabSelector = tabSelector;
        }

        public virtual void Initialize(UserTab tab)
            => base.Select(this, new UserTabSelectedEventArgs(Tab = tab, ChangeType.JumpTo));

        public override void Select(object sender, UserTabSelectedEventArgs eventArgs)
            => TabSelector.Select(sender, eventArgs);

        public new class Factory : PlaceholderFactory<BaseSelectableUserTabBehaviour> { }
    }
}