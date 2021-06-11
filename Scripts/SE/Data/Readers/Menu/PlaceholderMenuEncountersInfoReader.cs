namespace ClinicalTools.SimEncounters
{
    public class PlaceholderMenuEncountersInfoReader : IMenuEncountersInfoReader
    {
        public virtual WaitableTask<IMenuEncountersInfo> GetMenuEncountersInfo(User user)
        {
            // Usually, menu encounter data is retrieved before changing scenes.
            // This class is usually used to generate dummy data if a class would usually switch scenes,
            // but doesn't actually need to.
            ImageHolder.StopHoldingData();
            return new WaitableTask<IMenuEncountersInfo>();
        }
    }
}