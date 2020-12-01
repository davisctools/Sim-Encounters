using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TMPInputFieldOverrideScript : TMP_InputField
    {
        protected ScrollRect ScrollView {get; set;}

        // TextMeshPro glitches for fields added with data filled in them
        // Enabling and disabling the text component of it or a parent of it fixes the issue
        protected override void Awake()
        {
            base.Awake();
            textComponent.enabled = false;
        }

        protected override void Start()
        {
            base.Start();

            ScrollView = GetComponentInParent<ScrollRect>();
            textComponent.enabled = true;
        }

        // Allows the scroll view to scroll, even if this object is selected
        public override void OnScroll(PointerEventData eventData)
        {
            if (ScrollView != null && !transform.Find("Suggestions").gameObject.activeInHierarchy)
                ScrollView.OnScroll(eventData);
        }
    }
}