using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterWriterInstaller : MonoInstaller
    {
        protected FileManagerInstaller FileManagerInstaller { get; set; }

        public override void InstallBindings()
        {
            FileManagerInstaller = new FileManagerInstaller();
            InstallEncounterWriterBindings(Container);
        }

        protected virtual void InstallEncounterWriterBindings(DiContainer subcontainer)
        {
            foreach (SaveType saveType in Enum.GetValues(typeof(SaveType))) {
                subcontainer.Bind<IEncounterWriter>().WithId(saveType)
                    .FromSubContainerResolve().ByMethod(
                        (container) => BindEncounterWriterInstaller(container, saveType)).AsTransient();
            }
        }

        protected virtual void BindEncounterWriterInstaller(DiContainer subcontainer, SaveType saveType)
        {
            if (saveType == SaveType.Server) {
                subcontainer.Bind<IEncounterWriter>().To<ServerEncounterWriter>().AsTransient();
                return;
            }

            if (saveType == SaveType.Local) {
                subcontainer.Bind<IEncounterWriter>().To<LocalEncounterWriter>().AsTransient().WhenNotInjectedInto<LocalEncounterWriter>();
                subcontainer.Bind<IEncounterWriter>().To<EncounterFileWriter>().AsTransient().WhenInjectedInto<LocalEncounterWriter>();
                FileManagerInstaller.BindFileManagerWithId(subcontainer, SaveType.Local);
                FileManagerInstaller.BindFileManagerWithId(subcontainer, SaveType.Autosave);
            } else {
                subcontainer.Bind<IEncounterWriter>().To<EncounterFileWriter>().AsTransient();
            }
            subcontainer.Bind<IMetadataWriter>().To<LocalMetadataWriter>().AsTransient();
            FileManagerInstaller.BindFileManager(subcontainer, saveType);
        }
    }
}