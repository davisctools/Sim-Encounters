using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSelectorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<UserEncounterSelectorManager>().AsSingle();

            Container.BindInterfacesTo<LoadingReaderSceneInfoSelector>().AsSingle();
            Container.BindInterfacesTo<ReaderSceneInfoSelector>().AsSingle();

            Container.Bind<IFeedbackColorManager>().To<FeedbackColorManager>().AsTransient();

            Container.Bind<ILinearEncounterNavigator>().To<LinearUserEncounterNavigator>().AsSingle();
            Container.Bind<ICompletionHandler>().To<CompletionHandler>().AsSingle();
        }
    }
}