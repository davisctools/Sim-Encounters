using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MobileDesktopInstaller : MonoInstaller
    {
        public List<SubcontainerInstaller> DesktopInstallers { get => desktopInstallers; set => desktopInstallers = value; }
        [SerializeField] private List<SubcontainerInstaller> desktopInstallers;
        public List<SubcontainerInstaller> MobileInstallers { get => mobileInstallers; set => mobileInstallers = value; }
        [SerializeField] private List<SubcontainerInstaller> mobileInstallers;

        public override void InstallBindings()
        {
#if MOBILE
            foreach (var installer in MobileInstallers)
                installer.Install(Container);
#else
            foreach (var installer in DesktopInstallers)
                installer.Install(Container);
#endif
        }
    }
}