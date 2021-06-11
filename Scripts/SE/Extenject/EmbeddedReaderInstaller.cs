using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EmbeddedReaderInstaller : MonoInstaller
    {
        public virtual EmbeddedReaderSideController ReaderController { get => readerController; set => readerController = value; }
        [SerializeField] private EmbeddedReaderSideController readerController;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<EmbeddedReaderSideController>().FromInstance(ReaderController);
            // Prevents menu encounters from being loaded when choosing to "return to the main menu"
            Container.Bind<IMenuEncountersInfoReader>()
                         .To<PlaceholderMenuEncountersInfoReader>()
                         .AsTransient();
        }
    }
}