using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Scrollbar))]
    public class ScrollbarResetScript : MonoBehaviour
    {
        protected Scrollbar Scrollbar { get; set; }

        protected virtual void Start()
        {
            Scrollbar = GetComponent<Scrollbar>();
            NextFrame.Function(ResetScroll);
        }

        protected virtual void ResetScroll()
        {
            if (this != null)
                Scrollbar.value = 1;
        }

        protected virtual void Update()
        {
            if (Scrollbar.value > 1 || Scrollbar.value < 0)
                ResetScroll();
        }
    }
}