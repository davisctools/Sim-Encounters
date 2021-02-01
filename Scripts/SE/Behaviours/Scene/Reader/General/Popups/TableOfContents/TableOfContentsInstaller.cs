using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TableOfContentsInstaller : MonoInstaller
    {
        [SerializeField] private BaseTableOfContentsSection tableOfContentsSection;
        [SerializeField] private BaseTableOfContentsTab tableOfContentsTab;
        [SerializeField] private Transform parentTransform;

        public override void InstallBindings()
        {
            Container.BindFactory<BaseTableOfContentsSection, BaseTableOfContentsSection.Factory>()
                .FromComponentInNewPrefab(tableOfContentsSection)
                .UnderTransform(parentTransform);
            if (tableOfContentsTab != null) {
                Container.BindFactory<BaseTableOfContentsTab, BaseTableOfContentsTab.Factory>()
                    .FromComponentInNewPrefab(tableOfContentsTab)
                    .UnderTransform(parentTransform);
            }
        }
    }
}