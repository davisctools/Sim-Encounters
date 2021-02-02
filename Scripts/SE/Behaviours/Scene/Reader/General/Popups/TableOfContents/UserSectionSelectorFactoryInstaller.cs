using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorFactoryInstaller : MonoInstaller
    {
        [SerializeField] private BaseSelectableUserSectionBehaviour tableOfContentsSection;
        [SerializeField] private Transform parentTransform;

        public override void InstallBindings()
        {
            Container.BindFactory<BaseSelectableUserSectionBehaviour, BaseSelectableUserSectionBehaviour.Factory>()
                     .FromComponentInNewPrefab(tableOfContentsSection)
                     .UnderTransform(parentTransform);
        }
    }
}