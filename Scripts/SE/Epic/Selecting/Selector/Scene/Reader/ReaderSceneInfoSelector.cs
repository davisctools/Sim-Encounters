namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneInfoSelector : Selector<ReaderSceneInfoSelectedEventArgs>
    {
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; }
        public ReaderSceneInfoSelector(ISelector<UserEncounterSelectedEventArgs> userEncounterSelector)
            => UserEncounterSelector = userEncounterSelector;

        public override void Select(object sender, ReaderSceneInfoSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            UserEncounterSelector.Select(this, new UserEncounterSelectedEventArgs(eventArgs.SceneInfo.Encounter));
        }
    }
}