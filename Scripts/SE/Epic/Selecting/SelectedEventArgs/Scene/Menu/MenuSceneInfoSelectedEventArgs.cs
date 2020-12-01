using System;

namespace ClinicalTools.SimEncounters
{
    public class MenuSceneInfoSelectedEventArgs : EventArgs
    {
        public MenuSceneInfo SceneInfo { get; }
        public MenuSceneInfoSelectedEventArgs(MenuSceneInfo sceneInfo) => SceneInfo = sceneInfo;
    }
}