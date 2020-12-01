using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class OpenSidebarButton : MonoBehaviour
    {
        protected ISidebarController SidebarController { get; set; }
        [Inject] public void Inject(ISidebarController sidebarController) => SidebarController = sidebarController;

        protected virtual void Start() => GetComponent<Button>().onClick.AddListener(OpenSidebar);

        protected virtual void OpenSidebar() => SidebarController.Open();
    }
}