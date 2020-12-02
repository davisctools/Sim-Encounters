using System;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneInfoSelectedEventArgs : EventArgs
    {
        public ReaderSceneInfo SceneInfo { get; }
        public ReaderSceneInfoSelectedEventArgs(ReaderSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
}