using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorInstaller : MonoInstaller
    {
        public UserSectionSelectorBehaviour SelectorBehaviour { get => selectorBehaviour; set => selectorBehaviour = value; }
        [SerializeField] private UserSectionSelectorBehaviour selectorBehaviour;

        public override void InstallBindings() => Container.BindInterfacesTo<UserSectionSelectorBehaviour>()
                        .FromInstance(SelectorBehaviour)
                        .When(r => !ReferenceEquals(r.ObjectInstance, SelectorBehaviour));
    }
}