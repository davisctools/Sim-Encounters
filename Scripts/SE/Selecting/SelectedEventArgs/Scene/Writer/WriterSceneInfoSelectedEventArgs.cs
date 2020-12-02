using System;

namespace ClinicalTools.SimEncounters
{
    public class WriterSceneInfoSelectedEventArgs : EventArgs
    {
        public WriterSceneInfo SceneInfo { get; }
        public WriterSceneInfoSelectedEventArgs(WriterSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
}