using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ServerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IUrlBuilder>().To<UrlBuilder>().AsTransient();
            Container.Bind<IServerReader>().To<ServerReader>().AsTransient();
        }
    }
}