using ClinicalTools.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TagsFormatterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TagsFormatter>().To<TagsFormatter>().AsSingle();
            Container.Bind<UrlTagsFormatter>().To<UrlTagsFormatter>().AsSingle();
            Container.Bind<VisitedLinksManager>().To<VisitedLinksManager>().AsSingle();
        }
    }
}