namespace ClinicalTools.SimEncounters
{
    public class LoadingReaderSceneInfoSelector : Selector<LoadingReaderSceneInfoSelectedEventArgs>
    {
        protected ISelector<ReaderSceneInfoSelectedEventArgs> ReaderSceneInfoSelector { get; }
        public LoadingReaderSceneInfoSelector(ISelector<ReaderSceneInfoSelectedEventArgs> readerSceneInfoSelector)
            => ReaderSceneInfoSelector = readerSceneInfoSelector;

        public override void Select(object sender, LoadingReaderSceneInfoSelectedEventArgs value)
        {
            base.Select(sender, value);
            value.SceneInfo.Result.AddOnCompletedListener(SelectReaderSceneInfo);
        }

        protected virtual void SelectReaderSceneInfo(TaskResult<ReaderSceneInfo> sceneInfoResult)
        {
            CurrentValue.SceneInfo.LoadingScreen?.Stop();
            if (sceneInfoResult.HasValue())
                ReaderSceneInfoSelector.Select(this, new ReaderSceneInfoSelectedEventArgs(sceneInfoResult.Value));
        }
    }
}