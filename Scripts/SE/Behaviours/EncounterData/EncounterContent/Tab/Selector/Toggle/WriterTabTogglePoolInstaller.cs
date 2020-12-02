using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterTabTogglePoolInstaller : MonoInstaller
    {
        public BaseWriterTabToggle WriterTabTogglePrefab { get => writerTabTogglePrefab; set => writerTabTogglePrefab = value; }
        [SerializeField] private BaseWriterTabToggle writerTabTogglePrefab;
        public Transform PoolParent { get => poolParent; set => poolParent = value; }
        [SerializeField] private Transform poolParent;
        public int InitialSize { get => initialSize; set => initialSize = value; }
        [SerializeField] private int initialSize;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<BaseWriterTabToggle, BaseWriterTabToggle.Pool>()
                .WithInitialSize(InitialSize)
                .FromComponentInNewPrefab(WriterTabTogglePrefab)
                .UnderTransform(PoolParent);
        }
    }
}