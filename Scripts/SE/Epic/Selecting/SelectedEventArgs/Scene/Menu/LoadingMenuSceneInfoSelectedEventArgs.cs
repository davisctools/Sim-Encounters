using System;

namespace ClinicalTools.SimEncounters
{
    public class LoadingMenuSceneInfoSelectedEventArgs : EventArgs
    {
        public LoadingMenuSceneInfo SceneInfo { get; }
        public LoadingMenuSceneInfoSelectedEventArgs(LoadingMenuSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
}