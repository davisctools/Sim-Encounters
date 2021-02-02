using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TableOfContentsInstaller : MonoInstaller
    {
        [SerializeField] private BaseTableOfContentsSection tableOfContentsSection;
        [SerializeField] private BaseSelectableUserTabBehaviour tableOfContentsTab;
        [SerializeField] private Transform parentTransform;

        public override void InstallBindings()
        {
            Container.BindFactory<BaseTableOfContentsSection, BaseTableOfContentsSection.Factory>()
                .FromComponentInNewPrefab(tableOfContentsSection)
                .UnderTransform(parentTransform);
            if (tableOfContentsTab != null) {
                Container.BindFactory<BaseSelectableUserTabBehaviour, BaseSelectableUserTabBehaviour.Factory>()
                    .FromComponentInNewPrefab(tableOfContentsTab)
                    .UnderTransform(parentTransform);
            }
        }
    }
}