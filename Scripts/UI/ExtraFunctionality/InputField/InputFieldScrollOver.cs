using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class InputFieldScrollOver : TMP_InputField
    {
        // TextMeshPro glitches for fields added with data filled in them
        // Enabling and disabling the text component of it or a parent of it fixes the issue
        protected override void Awake()
        {
            base.Awake();

            textComponent.enabled = false;
        }

        private ScrollRect scrollView;
        protected override void Start()
        {
            base.Start();
            scrollView = GetComponentInParent<ScrollRect>();
            textComponent.enabled = true;
        }

        // Allows the scroll view to scroll, even if this object is selected
        public override void OnScroll(PointerEventData eventData)
        {
            if (scrollView != null)
                scrollView.OnScroll(eventData);
        }
    }
}