using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorTogglePoolInstaller : MonoInstaller
    {
        public virtual BaseSelectableTabToggle TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private BaseSelectableTabToggle tabButtonPrefab;

        public override void InstallBindings()
            => Container.BindMemoryPool<BaseSelectableTabToggle, SceneMonoMemoryPool<BaseSelectableTabToggle>>()
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(TabButtonPrefab);
    }
}