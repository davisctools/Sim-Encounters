using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    [RequireComponent(typeof(LayoutElement))]
    public class InputFieldResizer : MonoBehaviour
    {
        public float minimumHeight = 0;
        private float inputHeight = 0;
        private bool isPasting = true;

        protected RectTransform RectTransform => (RectTransform)transform;

        protected virtual TMP_InputField InputField { get; set; }
        protected LayoutElement LayoutElement { get; set; }

        protected virtual void Awake()
        {
            LayoutElement = GetComponent<LayoutElement>();
            InputField = GetComponent<TMP_InputField>();
        }
        void Start()
        {
            if (InputField.characterValidation.Equals(TMP_InputField.CharacterValidation.None))
                InputField.onValidateInput += MyValidate;
            NextFrame.Function(ResizeField2);
            InputField.onValueChanged.AddListener(ResizeField);
        }

        char MyValidate(string input, int charIndex, char charToValidate)
        {
            if (charToValidate == '	' || charToValidate == Convert.ToChar(8203))
                return '\0';
            return charToValidate;
        }

        // Ensures that the coroutine is only called once per frame.
        private bool resizeLock;
        public void ResizeField()
        {
            Debug.LogWarning($"{name}: Resize field called improperly");
        }
        protected virtual void ResizeField(string str) => ResizeField2();
        protected virtual void ResizeField2()
        {
            if (resizeLock)
                return;
            resizeLock = true;

            NextFrame.Function(ActivateResizeTMP);
            NextFrame.Function(delegate { NextFrame.Function(ActivateResizeTMP); });

            if (InputField)
                ZeroRectTransformOffsets(InputField.textComponent.rectTransform);
        }

        private RectTransform inputCaret;
        // Update is called once per frame
        void LateUpdate()
        {
            if (isPasting) {
                isPasting = false;
                ResizeField2();
            }

            // The input caret must be resized manually
            if (inputCaret == null) {
                var caret = GetComponentInChildren<TMP_SelectionCaret>();
                if (caret != null)
                    inputCaret = caret.rectTransform;
                else
                    return;
            }

            ZeroRectTransformOffsets(inputCaret);
        }

        private void ActivateResizeTMP()
        {
            resizeLock = false;

            if (isPasting)
                return;

            try {
                if (gameObject == null)
                    return;
            } catch {
                return;
            }

            var text = InputField.text;
            if (text.Length > 0 && text[text.Length - 1] == '\n')
                text += ' ';

            //Min = Left/Top, Max = Right/Bottom in the text's rect transform
            float verticalMargins = RectTransform.rect.width - InputField.textComponent.rectTransform.rect.width;
            float horizontalMargins = RectTransform.rect.height - InputField.textComponent.rectTransform.rect.height;

            inputHeight = LayoutElement.preferredHeight - horizontalMargins;

            float newInputHeight;
            try {
                float width = RectTransform.sizeDelta.x - verticalMargins;
                newInputHeight = InputField.textComponent.GetPreferredValues(text, width, 0).y;
            } catch (Exception e) {
                Debug.LogWarning("THE JUMBLED TEXT BUG JUST HAPPENED. ERROR MESSAGE: " + e.Message);

                return;
                //We could force a redraw here since this bug only seems to happen once in a blue moon for whatever reason
                //But for now I am just forcing a return to avoid breaking things further
            }

            if (newInputHeight == inputHeight)
                return;

            float height;
            if (newInputHeight > minimumHeight - horizontalMargins) {
                inputHeight = newInputHeight;
                height = inputHeight + horizontalMargins;
            } else {
                height = minimumHeight;
            }
            SetHeight(height);

            ZeroRectTransformOffsets(InputField.textComponent.rectTransform);
        }

        protected virtual void ZeroRectTransformOffsets(RectTransform rectTransform)
        {
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        }


        protected virtual void SetHeight(float height)
        {
            LayoutElement.minHeight = height;
            LayoutElement.preferredHeight = height;
        }
    }
}