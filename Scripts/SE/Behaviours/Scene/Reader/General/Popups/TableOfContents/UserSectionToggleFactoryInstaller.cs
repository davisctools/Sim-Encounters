using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionToggleFactoryInstaller : MonoInstaller
    {
        [SerializeField] private UserSectionToggle sectionToggle;

        public override void InstallBindings()
            => Container.BindFactory<UserSectionToggle, PlaceholderFactory<UserSectionToggle>>()
                        .FromComponentInNewPrefab(sectionToggle);
    }
}