namespace ClinicalTools.SimEncounters
{
    public interface IMenuEncountersInfoReader
    {
        WaitableTask<IMenuEncountersInfo> GetMenuEncountersInfo(User user);
    }
}