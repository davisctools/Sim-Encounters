using UnityEngine;

namespace ClinicalTools.UI
{
    public class ControlActive : MonoBehaviour
    {
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void SetActive(bool value) => gameObject.SetActive(value);
        public virtual void SetInactive(bool value) => gameObject.SetActive(!value);
    }
}