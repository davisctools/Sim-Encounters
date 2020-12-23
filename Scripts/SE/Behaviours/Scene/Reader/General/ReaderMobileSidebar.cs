using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSidebar : MonoBehaviour, ICloseHandler
    {
        protected ISidebarController SidebarController { get; set; }
        [Inject] public virtual void Inject(ISidebarController sidebarController) => SidebarController = sidebarController;
        public void Close(object sender) => SidebarController.Close();
    }
}
