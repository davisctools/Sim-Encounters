using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SectionButtonInstaller : MonoInstaller
    {
        public virtual ReaderSectionToggle SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }
        [SerializeField] private ReaderSectionToggle sectionButtonPrefab;

        public override void InstallBindings()
            => Container.BindFactory<ReaderSectionToggle, ReaderSectionToggle.Factory>()
                        .FromComponentInNewPrefab(SectionButtonPrefab);
    }
}