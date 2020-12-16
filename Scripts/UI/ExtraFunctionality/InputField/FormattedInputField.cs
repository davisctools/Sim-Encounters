using System;
using UnityEngine.EventSystems;
using Zenject;

namespace ClinicalTools.UI
{
    public class FormattedInputField : InputFieldScrollOver
    {
        protected string UnformattedText { get; set; }
        protected string FormattedText { get; set; }


        protected virtual TagsFormatter TagsFormatter { get; set; }
        [Inject] public virtual void Inject(TagsFormatter tagsFormatter) => TagsFormatter = tagsFormatter;
        
        public virtual void SetUnformattedText(string text)
        {
            UpdateFormattedText(text);
            this.text = richText ? FormattedText : UnformattedText;
        }
        public virtual string GetUnformattedText() => UnformattedText;

        protected override void Start()
        {
            if (characterValidation.Equals(CharacterValidation.None))
                onValidateInput += MyValidate;

            base.Start();
        }

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
            FormattedText = TagsFormatter.ProcessText(text);
        }


        protected virtual char MyValidate(string input, int charIndex, char charToValidate)
        {
            if (charToValidate  == '\r' || charToValidate == '	' || charToValidate == Convert.ToChar(8203))
                return '\0';

            return charToValidate;
        }
    }
}