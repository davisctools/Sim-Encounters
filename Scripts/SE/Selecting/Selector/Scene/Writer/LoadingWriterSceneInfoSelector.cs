namespace ClinicalTools.SimEncounters
{
    public class LoadingWriterSceneInfoSelector : Selector<LoadingWriterSceneInfoSelectedEventArgs>
    {
        protected ISelector<WriterSceneInfoSelectedEventArgs> SceneInfoSelector { get; }
        public LoadingWriterSceneInfoSelector(ISelector<WriterSceneInfoSelectedEventArgs> sceneInfoSelector)
            => SceneInfoSelector = sceneInfoSelector;

        public override void Select(object sender, LoadingWriterSceneInfoSelectedEventArgs value)
        {
            base.Select(sender, value);
            value.SceneInfo.Result.AddOnCompletedListener(SelectReaderSceneInfo);
        }

        protected virtual void SelectReaderSceneInfo(TaskResult<WriterSceneInfo> sceneInfoResult)
        {
            if (sceneInfoResult.HasValue())
                SceneInfoSelector.Select(this, new WriterSceneInfoSelectedEventArgs(sceneInfoResult.Value));
        }
    }
}