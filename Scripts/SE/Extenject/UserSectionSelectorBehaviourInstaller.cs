using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorBehaviourInstaller : MonoInstaller
    {
        [SerializeField] private BaseSelectableUserSectionBehaviour sectionSelector;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BaseSelectableUserSectionBehaviour>()
                     .FromInstance(sectionSelector)
                     .WhenNotInjectedInto<BaseSelectableUserSectionBehaviour>();
        }
    }
}