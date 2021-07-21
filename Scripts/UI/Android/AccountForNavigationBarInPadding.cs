using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(LayoutGroup))]
    public class AccountForNavigationBarInPadding : MonoBehaviour
    {
        protected LayoutGroup Group { get; set; }
        protected int DefaultPadding { get; set; }

        protected virtual void Start()
        {
            Group = GetComponent<LayoutGroup>();
            DefaultPadding = Group.padding.bottom;
        }

        protected virtual void Update()
            => Group.padding.bottom = DefaultPadding + AndroidStatusBarManager.NavigationBarHeight;
    }
}