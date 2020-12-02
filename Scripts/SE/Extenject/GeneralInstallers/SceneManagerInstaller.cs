using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SceneManagerInstaller : SubcontainerInstaller
    {
        public string MenuScene { get => menuScene; set => menuScene = value; }
        [SerializeField] private string menuScene;
        public string ReaderScene { get => readerScene; set => readerScene = value; }
        [SerializeField] private string readerScene;
        public string WriterScene { get => writerScene; set => writerScene = value; }
        [SerializeField] private string writerScene;

        public override void Install(DiContainer container)
        {
            container.Bind<IMenuSceneStarter>().To<MenuSceneStarter>().AsTransient();
            container.Bind<IReaderSceneStarter>().To<ReaderSceneStarter>().AsTransient();
            container.Bind<IWriterSceneStarter>().To<WriterSceneStarter>().AsTransient();
            container.Bind<IEncounterQuickStarter>().To<EncounterQuickStarter>().AsTransient();
            container.Bind<QuickActionFactory>().To<QuickActionFactory>().AsTransient();

            container.Bind<string>().FromInstance(WriterScene).WhenInjectedInto<WriterSceneStarter>();
            container.Bind<string>().FromInstance(MenuScene).WhenInjectedInto<MenuSceneStarter>();
            container.Bind<string>().FromInstance(ReaderScene).WhenInjectedInto<ReaderSceneStarter>();
        }
    }
}