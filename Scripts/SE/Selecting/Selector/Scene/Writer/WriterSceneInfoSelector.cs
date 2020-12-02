namespace ClinicalTools.SimEncounters
{
    public class WriterSceneInfoSelector : Selector<WriterSceneInfoSelectedEventArgs>
    {
        protected ISelector<EncounterSelectedEventArgs> EncounterSelector { get; }
        public WriterSceneInfoSelector(ISelector<EncounterSelectedEventArgs> encounterSelector)
            => EncounterSelector = encounterSelector;

        public override void Select(object sender, WriterSceneInfoSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            EncounterSelector.Select(this, new EncounterSelectedEventArgs(eventArgs.SceneInfo.Encounter));
        }
    }
}