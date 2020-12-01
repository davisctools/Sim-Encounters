using UnityEngine;

namespace ClinicalTools.UI
{
    public class CloseablePopup : MonoBehaviour, ICloseHandler
    {
        public virtual void Close(object sender) => gameObject.SetActive(false);
    }
}