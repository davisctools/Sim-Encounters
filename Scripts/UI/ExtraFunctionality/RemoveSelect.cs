using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    /// <summary>
    /// Whenever this object is enabled, it ensures no field is still selected.
    /// Useful for when opening a panel using a button to prevent the panel from being opened multiple times.
    /// </summary>
    public class RemoveSelect : MonoBehaviour
    {
        private void OnEnable() => EventSystem.current.SetSelectedGameObject(null);
    }
}