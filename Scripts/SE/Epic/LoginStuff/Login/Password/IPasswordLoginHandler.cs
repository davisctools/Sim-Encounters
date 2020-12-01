namespace ClinicalTools.SimEncounters
{
    public interface IPasswordLoginHandler
    {
        WaitableTask<User> Login(string username, string email, string password);
    }
}