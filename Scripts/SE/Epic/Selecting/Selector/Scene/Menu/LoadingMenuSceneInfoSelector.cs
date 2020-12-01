namespace ClinicalTools.SimEncounters
{
    public class LoadingMenuSceneInfoSelector : Selector<LoadingMenuSceneInfoSelectedEventArgs>
    {
        protected ISelector<MenuSceneInfoSelectedEventArgs> MenuSceneInfoSelector { get; }
        public LoadingMenuSceneInfoSelector(ISelector<MenuSceneInfoSelectedEventArgs> menuSceneInfoSelector)
            => MenuSceneInfoSelector = menuSceneInfoSelector;

        public override void Select(object sender, LoadingMenuSceneInfoSelectedEventArgs value)
        {
            base.Select(sender, value);
            value.SceneInfo.Result.AddOnCompletedListener(SelectReaderSceneInfo);
        }

        protected virtual void SelectReaderSceneInfo(TaskResult<MenuSceneInfo> sceneInfoResult)
        {
            CurrentValue.SceneInfo.LoadingScreen?.Stop();
            if (sceneInfoResult.HasValue())
                MenuSceneInfoSelector.Select(this, new MenuSceneInfoSelectedEventArgs(sceneInfoResult.Value));
        }
    }
}