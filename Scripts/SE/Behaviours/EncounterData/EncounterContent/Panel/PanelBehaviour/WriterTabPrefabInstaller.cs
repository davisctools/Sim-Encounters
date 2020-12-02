using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterTabPrefabInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<string, BaseTabDrawer, BaseTabDrawer.Factory>()
                      .FromFactory<PrefabResourceFactory<BaseTabDrawer>>();
            Container.BindFactory<Object, BaseWriterPanel, BaseWriterPanel.Factory>()
                      .FromFactory<PrefabFactory<BaseWriterPanel>>();
        }
    }
}