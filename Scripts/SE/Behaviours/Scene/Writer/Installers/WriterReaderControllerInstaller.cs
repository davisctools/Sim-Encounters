using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterReaderControllerInstaller : MonoInstaller
    {
        public virtual WriterReaderSideController WriterController { get => dropdownOptionPrefab; set => dropdownOptionPrefab = value; }
        [SerializeField] private WriterReaderSideController dropdownOptionPrefab;

        public override void InstallBindings()
            => Container.BindInstance<IReaderSceneStarter>(WriterController);
    }
}