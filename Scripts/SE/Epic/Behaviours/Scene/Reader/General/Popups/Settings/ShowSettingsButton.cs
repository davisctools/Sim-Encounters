using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class ShowSettingsButton : MonoBehaviour
    {
        private Button button;
        protected Button Button
        {
            get {
                if (button == null)
                    button = GetComponent<Button>();
                return button;
            }
        }

        protected BaseSettingsPopup SettingsPopup { get; set; }
        [Inject]
        public void Inject(BaseSettingsPopup settingsPopup)
            => SettingsPopup = settingsPopup;

        protected virtual void Awake() => Button.onClick.AddListener(ShowEncounterInfo);
        public virtual void ShowEncounterInfo() => SettingsPopup.ShowSettings();
    }
}