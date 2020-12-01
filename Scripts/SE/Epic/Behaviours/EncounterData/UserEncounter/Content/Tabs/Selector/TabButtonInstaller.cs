using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabButtonInstaller : MonoInstaller
    {
        public virtual BaseReaderTabToggle TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private BaseReaderTabToggle tabButtonPrefab;

        public override void InstallBindings()
            => Container.BindMemoryPool<BaseReaderTabToggle, BaseReaderTabToggle.Pool>()
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(TabButtonPrefab);
    }
}