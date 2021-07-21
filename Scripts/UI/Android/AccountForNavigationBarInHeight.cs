using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(LayoutElement))]
    public class AccountForNavigationBarInHeight : MonoBehaviour
    {
        protected LayoutElement Element { get; set; }
        protected float DefaultHeight { get; set; }

        protected virtual void Start()
        {
            Element = GetComponent<LayoutElement>();
            DefaultHeight = Element.preferredHeight;
        }

        protected virtual void Update() => Element.preferredHeight = DefaultHeight + AndroidStatusBarManager.NavigationBarHeight;
    }
}