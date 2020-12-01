using UnityEngine;

namespace ClinicalTools.UI
{
    public class TextDropdownTabField : TabField
    {
        private GameObject suggestions;
        private RectTransform content;

        protected override void Start()
        {
            var suggestionsTransform = transform.Find("Suggestions");
            if (suggestionsTransform != null)
                suggestions = suggestionsTransform.gameObject;
            content = (RectTransform)transform.Find("Suggestions/Viewport/Content");

            base.Start();
        }

        protected override void Update()
        {
            SetWasSelected();
            base.Update();
            SetWasSelected();
        }

        protected virtual void SetWasSelected()
        {
            if (suggestions && suggestions.activeSelf && content.rect.height > 0)
                WasSelected = false;
        }
    }
}