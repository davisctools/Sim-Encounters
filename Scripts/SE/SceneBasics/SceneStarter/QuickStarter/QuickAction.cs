namespace ClinicalTools.SimEncounters
{
    public enum QuickActionType { NA, MainMenu, Reader }
    public class QuickAction
    {
        public QuickActionType Action { get; }
        public int EncounterId { get; }

        public QuickAction()
        {
            Action = QuickActionType.NA;
        }
        public QuickAction(QuickActionType action)
        {
            Action = action;
        }
        public QuickAction(QuickActionType action, int id)
        {
            Action = action;
            EncounterId = id;
        }
    }
}