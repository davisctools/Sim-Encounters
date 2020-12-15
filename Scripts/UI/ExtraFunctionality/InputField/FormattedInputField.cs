using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    public class FormattedInputField : InputFieldScrollOver
    {
        protected string UnformattedText { get; set; }
        protected string FormattedText { get; set; }

        public virtual void SetUnformattedText(string text)
        {
            UpdateFormattedText(text);
            this.text = richText ? FormattedText : UnformattedText;
        }
        public virtual string GetUnformattedText() => UnformattedText;

        public override void OnSelect(BaseEventData eventData)
        {
            richText = false;
            text = UnformattedText;
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            richText = true;
            UpdateFormattedText(text);
            text = FormattedText;
            base.OnDeselect(eventData);
        }

        protected virtual void UpdateFormattedText(string text)
        {
            UnformattedText = text;
            var listsReplacer = new ListTagsReplacer();
            FormattedText = listsReplacer.ReplaceLists(text);
        }
    }
}