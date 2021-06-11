using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EmbeddedReaderControllerInstaller : MonoInstaller
    {
        public virtual EmbeddedReaderSideController ReaderController { get => readerController; set => readerController = value; }
        [SerializeField] private EmbeddedReaderSideController readerController;

        public override void InstallBindings()
            => Container.BindInstance<IReaderSceneStarter>(ReaderController);
    }
}