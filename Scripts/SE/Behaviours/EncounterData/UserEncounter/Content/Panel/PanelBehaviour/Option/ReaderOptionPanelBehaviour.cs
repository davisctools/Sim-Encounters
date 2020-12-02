namespace ClinicalTools.SimEncounters
{
    public abstract class ReaderOptionPanelBehaviour : BaseReaderPanelBehaviour
    {
        public virtual void GetFeedback()
        {
            CurrentPanel.SetChildPanelsRead(true);
        }
    }
}