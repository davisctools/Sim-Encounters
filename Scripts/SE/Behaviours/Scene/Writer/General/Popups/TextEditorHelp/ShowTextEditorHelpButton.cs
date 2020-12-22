using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class ShowTextEditorHelpButton : MonoBehaviour
    {
        protected GameObject TextEditorHelpPopup { get; set; }
        [Inject] public void Inject(GameObject textEditorHelpPopup) => TextEditorHelpPopup = textEditorHelpPopup;

        protected virtual void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(ShowTextEditorHelp);
        }

        protected virtual void ShowTextEditorHelp() => TextEditorHelpPopup.SetActive(true);
    }
}