using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class EditReaderCaseButton : MonoBehaviour
    {
        protected Button Button => (button == null) ? button = GetComponent<Button>() : button;
        private Button button;

        protected IWriterSceneStarter WriterSceneStarter { get; set; }
        protected ISelector<ReaderSceneInfoSelectedEventArgs> SceneInfoSelector { get; set; }
        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }

        [Inject]
        public void Inject(
            IWriterSceneStarter writerSceneStarter,
            ISelector<ReaderSceneInfoSelectedEventArgs> sceneInfoSelector,
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector)
        {
            WriterSceneStarter = writerSceneStarter;
            SceneInfoSelector = sceneInfoSelector;

            EncounterSelector = encounterSelector;
            EncounterSelector.Selected += OnEncounterSelected;
            if (EncounterSelector.CurrentValue != null)
                OnEncounterSelected(EncounterSelector, EncounterSelector.CurrentValue);
        }

        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs e)
            => gameObject.SetActive(e.Encounter.Data.Metadata.Author.Id == e.Encounter.User.AccountId);

        protected virtual void Awake() => Button.onClick.AddListener(ShowInstructions);
        public virtual void ShowInstructions()
        {
            var sceneInfo = SceneInfoSelector.CurrentValue.SceneInfo;
            var encounter = new WaitableTask<Encounter>(sceneInfo.Encounter.Data);
            var writerSceneInfo = new LoadingWriterSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            WriterSceneStarter.StartScene(writerSceneInfo);
        }
    }
}