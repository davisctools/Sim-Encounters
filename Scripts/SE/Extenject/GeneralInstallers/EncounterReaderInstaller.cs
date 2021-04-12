using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterReaderInstaller : MonoInstaller
    {
        protected FileManagerInstaller FileManagerInstaller { get; set; }

        public override void InstallBindings()
        {
            FileManagerInstaller = new FileManagerInstaller();
            InstallMenuReaderBindings(Container);
            InstallEncounterReaderBindings(Container);
        }

        protected virtual void InstallMenuReaderBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IMenuEncountersInfoReader>()
                        .To<MenuEncountersInfoReader>()
                        .AsTransient();
            subcontainer.Bind<IMenuEncountersReader>()
                        .To<MenuEncountersReader>()
                        .AsTransient();
            InstallMetadataReaderBindings(subcontainer);

            subcontainer.Bind<IEncounterLocksReader>()
                        .To<EncounterLocksReader>()
                        .AsTransient();

            subcontainer.Bind<IBasicStatusesReader>()
                        .To<BasicStatusesReader>()
                        .AsTransient()
                        .WhenNotInjectedInto<BasicStatusesReader>();
            subcontainer.Bind<IBasicStatusesReader>()
                        .To<LocalBasicStatusesReader>()
                        .AsTransient()
                        .WhenInjectedInto<BasicStatusesReader>();
            FileManagerInstaller.BindFileManager(subcontainer, SaveType.Local);
        }

        protected virtual void InstallMetadataReaderBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IMetadataReader>()
                        .FromSubContainerResolve()
                        .ByMethod(InstallMetadataReader)
                        .AsTransient();

            subcontainer.Bind<IMetadataGroupsReader>()
                        .To<MetadataGroupsReader>()
                        .AsTransient();

            foreach (SaveType saveType in Enum.GetValues(typeof(SaveType))) {
                subcontainer.Bind<IMetadatasReader>()
                            .WithId(saveType)
                            .FromSubContainerResolve()
                            .ByMethod((container) => BindMetadatasReaderInstaller(container, saveType))
                            .AsTransient();
            }
        }

        protected virtual void InstallMetadataReader(DiContainer subcontainer)
        {
            subcontainer.Bind<IMetadataReader>()
                        .To<LocalMetadataReader>()
                        .AsTransient();
            FileManagerInstaller.BindFileManager(subcontainer, SaveType.Demo);
        }

        protected virtual void BindMetadatasReaderInstaller(DiContainer subcontainer, SaveType saveType)
        {
            if (saveType == SaveType.Server) {
                subcontainer.Bind<IMetadatasReader>()
                            .To<EncountersMetadataReader>()
                            .AsTransient();
                subcontainer.Bind<IEncountersMetadataJsonRetriever>()
                            .To<ServerEncountersMetadataJsonRetriever>()
                            .AsTransient();
            } else {
                subcontainer.Bind<IMetadatasReader>()
                            .To<LocalMetadatasReader>()
                            .AsTransient();
                FileManagerInstaller.BindFileManager(subcontainer, saveType);
            }
        }

        protected virtual void InstallEncounterReaderBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IUserEncounterReader>()
                        .To<UserEncounterReader>()
                        .AsTransient();
            subcontainer.Bind<IDetailedStatusReader>()
                        .To<LocalDetailedStatusReader>()
                        .AsTransient();
            subcontainer.Bind<IEncounterReader>()
                        .To<EncounterReader>()
                        .AsTransient();
            InstallEncounterDataReaderBindings(subcontainer);
        }

        protected virtual void InstallEncounterDataReaderBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IEncounterDataReaderSelector>()
                        .To<EncounterDataReaderSelector>()
                        .AsTransient();

            foreach (SaveType saveType in Enum.GetValues(typeof(SaveType))) {
                subcontainer.Bind<IEncounterDataReader>()
                            .WithId(saveType)
                            .FromSubContainerResolve()
                            .ByMethod((container) => BindEncounterDataReaderInstaller(container, saveType))
                            .AsTransient();
            }
        }

        protected virtual void BindEncounterDataReaderInstaller(DiContainer subcontainer, SaveType saveType)
        {
            if (saveType == SaveType.Server)
                BindServerEncounterDataReaderInstaller(subcontainer);
            else
                BindLocalEncounterDataReaderInstaller(subcontainer, saveType);
        }


        protected virtual void BindServerEncounterDataReaderInstaller(DiContainer subcontainer)
        {
            subcontainer.Bind<IEncounterDataReader>()
                        .To<EncounterDataReader>()
                        .AsTransient();
            subcontainer.Bind<IEncounterDataTextRetriever>()
                        .To<ServerEncounterDataTextRetriever>()
                        .AsTransient();
            subcontainer.Bind<IEncounterImagesReader>()
                        .To<EncounterImagesReader>()
                        .AsTransient();
            subcontainer.Bind<IEncounterImagesJsonRetriever>()
                        .To<ServerEncounterImagesJsonRetriever>()
                        .AsTransient();
        }
        protected virtual void BindLocalEncounterDataReaderInstaller(DiContainer subcontainer, SaveType saveType)
        {
            subcontainer.Bind<IEncounterDataReader>()
                        .To<LegacyEncounterDataReader>()
                        .AsTransient()
                        .WhenNotInjectedInto<LegacyEncounterDataReader>();
            subcontainer.Bind<IEncounterDataReader>()
                        .To<LocalLegacyEncounterContentReader>()
                        .AsTransient()
                        .WhenInjectedInto<LegacyEncounterDataReader>();
            subcontainer.Bind<IImageContentReader>()
                        .To<LocalImageContentReader>()
                        .AsTransient();
            FileManagerInstaller.BindFileManager(subcontainer, saveType);
        }



    }
}