using System;

namespace ClinicalTools.SimEncounters
{
    public class LoadingWriterSceneInfoSelectedEventArgs : EventArgs
    {
        public LoadingWriterSceneInfo SceneInfo { get; }
        public LoadingWriterSceneInfoSelectedEventArgs(LoadingWriterSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
}