using System;

namespace ClinicalTools.SimEncounters
{
    public class LoadingReaderSceneInfoSelectedEventArgs : EventArgs
    {
        public LoadingReaderSceneInfo SceneInfo { get; }
        public LoadingReaderSceneInfoSelectedEventArgs(LoadingReaderSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
}