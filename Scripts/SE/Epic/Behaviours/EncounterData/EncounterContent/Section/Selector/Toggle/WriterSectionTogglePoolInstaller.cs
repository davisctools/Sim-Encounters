using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSectionTogglePoolInstaller : MonoInstaller
    {
        public BaseWriterSectionToggle WriterSectionTogglePrefab { get => writerSectionTogglePrefab; set => writerSectionTogglePrefab = value; }
        [SerializeField] private BaseWriterSectionToggle writerSectionTogglePrefab;
        public Transform PoolParent { get => poolParent; set => poolParent = value; }
        [SerializeField] private Transform poolParent;
        public int InitialSize { get => initialSize; set => initialSize = value; }
        [SerializeField] private int initialSize;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<BaseWriterSectionToggle, BaseWriterSectionToggle.Pool>()
                .WithInitialSize(InitialSize)
                .FromComponentInNewPrefab(WriterSectionTogglePrefab)
                .UnderTransform(PoolParent);
        }
    }
}