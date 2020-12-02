namespace ClinicalTools.SimEncounters
{
    public interface ILoginHandler
    {
        WaitableTask<User> Login();
    }
}