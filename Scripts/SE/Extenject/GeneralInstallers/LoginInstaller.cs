using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class LoginInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StayLoggedIn>().To<StayLoggedIn>().AsTransient();
            Container.Bind<ILoginHandler>().To<AutoLogin>().AsTransient();
            Container.Bind<IPasswordLoginHandler>().To<PasswordLogin>().AsTransient();
            Container.Bind<UserDeserializer>().To<UserDeserializer>().AsTransient();
        }
    }
}