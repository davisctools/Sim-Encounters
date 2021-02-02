using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorButtonPoolInstaller : MonoInstaller
    {
        public virtual BaseSelectableUserTabBehaviour TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private BaseSelectableUserTabBehaviour tabButtonPrefab;
        public virtual int InitialSize { get => initialSize; set => initialSize = value; }
        [SerializeField] private int initialSize = 10;

        public override void InstallBindings()
            => Container.BindMemoryPool<BaseSelectableUserTabBehaviour, SceneMonoMemoryPool<BaseSelectableUserTabBehaviour>>()
                        .WithInitialSize(InitialSize)
                        .FromComponentInNewPrefab(TabButtonPrefab);
    }
}