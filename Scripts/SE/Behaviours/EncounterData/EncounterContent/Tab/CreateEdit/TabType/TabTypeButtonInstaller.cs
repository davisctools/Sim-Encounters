using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabTypeButtonInstaller : MonoInstaller
    {
        public virtual TabTypeButtonUI TabTypeButtonPrefab { get => tabTypeButtonPrefab; set => tabTypeButtonPrefab = value; }
        [SerializeField] private TabTypeButtonUI tabTypeButtonPrefab;

        public override void InstallBindings()
            => Container.BindFactory<TabTypeButtonUI, TabTypeButtonUI.Factory>()
                    .FromComponentInNewPrefab(TabTypeButtonPrefab);
    }
}