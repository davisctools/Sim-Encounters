using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorFactoryInstaller : MonoInstaller
    {
        [SerializeField] private BaseSelectableUserSectionBehaviour tableOfContentsSection;
        [SerializeField] private Transform parentTransform;

        public override void InstallBindings() 
            => Container.BindFactory<BaseSelectableUserSectionBehaviour, PlaceholderFactory<BaseSelectableUserSectionBehaviour>>()
                        .FromComponentInNewPrefab(tableOfContentsSection)
                        .UnderTransform(parentTransform);
    }
}