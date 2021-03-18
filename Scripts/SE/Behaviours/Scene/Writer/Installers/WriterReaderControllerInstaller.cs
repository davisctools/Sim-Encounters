using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterReaderControllerInstaller : MonoInstaller
    {
        public virtual WriterReaderSideController WriterController { get => writerController; set => writerController = value; }
        [SerializeField] private WriterReaderSideController writerController;

        public override void InstallBindings()
            => Container.BindInstance<IReaderSceneStarter>(WriterController);
    }
}