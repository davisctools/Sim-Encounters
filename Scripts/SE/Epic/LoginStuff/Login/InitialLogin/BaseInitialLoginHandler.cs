namespace ClinicalTools.SimEncounters
{
    public abstract class BaseInitialLoginHandler : BaseLoginHandler
    {
        public abstract WaitableTask<User> InitialLogin(ILoadingScreen loadingScreen);
    }
}