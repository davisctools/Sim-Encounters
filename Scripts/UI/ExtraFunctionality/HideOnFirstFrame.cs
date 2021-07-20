using UnityEngine;

namespace ClinicalTools.UI
{
    public class HideOnFirstFrame : MonoBehaviour
    {
        protected virtual void Start() => gameObject.SetActive(false);
    }
}