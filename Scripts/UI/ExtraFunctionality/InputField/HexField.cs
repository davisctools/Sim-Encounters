using TMPro;
using UnityEngine;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class HexField : MonoBehaviour
    {
        private TMP_InputField input;

        private void Start()
        {
            input = GetComponent<TMP_InputField>();
            input.onValueChanged.AddListener(UpdateField);
        }

        private readonly string hexChars = "0123456789abcdefABCDEF";
        private void UpdateField(string text)
        {
            if (!input)
                return;

            var newText = "";

            foreach (var ch in text) {
                if (hexChars.Contains("" + ch))
                    newText += ch;
            }

            input.text = newText;
        }
    }
}