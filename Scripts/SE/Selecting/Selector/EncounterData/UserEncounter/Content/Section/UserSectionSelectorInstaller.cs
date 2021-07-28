using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorInstaller : MonoInstaller
    {
        public UserSectionSelectorBehaviour SelectorBehaviour { get => selectorBehaviour; set => selectorBehaviour = value; }
        [SerializeField] private UserSectionSelectorBehaviour selectorBehaviour;

        public override void InstallBindings()
        {
            Container.Bind<ISelectedListener<UserSectionSelectedEventArgs>>()
                     .To<UserSectionSelectorBehaviour>()
                     .FromInstance(SelectorBehaviour)
                     .When(r => !ReferenceEquals(r.ObjectInstance, SelectorBehaviour));
            Container.Bind<ISelectedListener<SectionSelectedEventArgs>>()
                     .To<UserSectionSelectorBehaviour>()
                     .FromInstance(SelectorBehaviour)
                     .When(r => !ReferenceEquals(r.ObjectInstance, SelectorBehaviour));
            Container.Bind<ISelectedListener<UserTabSelectedEventArgs>>()
                     .To<UserSectionSelectorBehaviour>()
                     .FromInstance(SelectorBehaviour)
                     .When(r => !ReferenceEquals(r.ObjectInstance, SelectorBehaviour));
            Container.Bind<ISelectedListener<TabSelectedEventArgs>>()
                     .To<UserSectionSelectorBehaviour>()
                     .FromInstance(SelectorBehaviour)
                     .When(r => !ReferenceEquals(r.ObjectInstance, SelectorBehaviour));
        }
    }
}