using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class WriterOpenSavePanelButton : MonoBehaviour
    {
        protected BaseSaveEncounterDisplay SavePopup { get; set; }
        protected ISelector<WriterSceneInfoSelectedEventArgs> SceneInfoSelector { get; set; }
        [Inject]
        public virtual void Inject(BaseSaveEncounterDisplay savePopup, ISelector<WriterSceneInfoSelectedEventArgs> sceneInfoSelector)
        {
            SavePopup = savePopup;
            SceneInfoSelector = sceneInfoSelector;
        }

        protected virtual Button Button { get; set; }
        protected virtual void Start()
        {
            Button = GetComponent<Button>();
            Button.interactable = false;
            Button.onClick.AddListener(ShowSavePopup);

            SceneInfoSelector.Selected += SceneLoaded;
            if (SceneInfoSelector.CurrentValue != null)
                SceneLoaded(this, SceneInfoSelector.CurrentValue);
        }

        private void SceneLoaded(object sender, WriterSceneInfoSelectedEventArgs e)
        {
            Button.interactable = true;
            Button.onClick.AddListener(ShowSavePopup);
        }

        protected virtual void ShowSavePopup()
        {
            var sceneInfo = SceneInfoSelector.CurrentValue.SceneInfo;
            SavePopup.Display(sceneInfo.User, sceneInfo.Encounter);
        }
    }
}