using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseSelectableUserSectionBehaviour : UserSectionSelectorBehaviour
    {
        protected virtual ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        protected virtual UserSection Section { get; set; }

        [Inject]
        public virtual void Inject(ISelector<UserSectionSelectedEventArgs> sectionSelector)
        {
            if (sectionSelector is BaseSelectableUserTabBehaviour s && s == this)
                throw new Exception("Injected Section Selector is itself. " +
                    $"Are you binding it correctly with {typeof(UserSectionSelectorInstaller).Name}?");

            SectionSelector = sectionSelector;
        }

        public virtual void Initialize(UserSection section)
            => base.Select(this, new UserSectionSelectedEventArgs(Section = section, ChangeType.JumpTo));

        public override void Select(object sender, UserSectionSelectedEventArgs eventArgs)
            => SectionSelector.Select(sender, eventArgs);
        public override void Select(object sender, UserTabSelectedEventArgs eventArgs)
        {
            SectionValue.SelectedSection.SetCurrentTab(eventArgs.SelectedTab.Data);
            SectionSelector.Select(sender, UserSectionValue);
        }

        public new class Factory : PlaceholderFactory<BaseSelectableUserSectionBehaviour> { }
    }
}