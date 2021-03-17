using UnityEngine;

namespace ClinicalTools.UI
{
    public abstract class BaseTooltip : MonoBehaviour
    {
        public abstract void Show();
        public abstract void Hide();
    }
}