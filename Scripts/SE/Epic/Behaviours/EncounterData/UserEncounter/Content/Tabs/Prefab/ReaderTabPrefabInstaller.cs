using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabPrefabInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Object, BaseReaderPanelBehaviour, BaseReaderPanelBehaviour.Factory>()
                      .FromFactory<PrefabFactory<BaseReaderPanelBehaviour>>();
            Container.BindFactory<string, UserTabSelectorBehaviour, UserTabSelectorBehaviour.Factory>()
                      .FromFactory<PrefabResourceFactory<UserTabSelectorBehaviour>>();
        }
    }
}