using ClinicalTools.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class WriterSaveAndViewInReaderButton : MonoBehaviour
    {
        protected SignalBus SignalBus { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }

        protected ISelectedListener<WriterSceneInfoSelectedEventArgs> SceneInfoSelector { get; set; }
        protected ISelectedListener<SectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelectedListener<TabSelectedEventArgs> TabSelector { get; set; }

        protected IReaderSceneStarter ReaderSceneStarter { get; set; }
        protected IEncounterWriter EncounterWriter { get; set; }
        [Inject]
        public virtual void Inject(
            SignalBus signalBus,
            BaseConfirmationPopup confirmationPopup,
            ISelectedListener<WriterSceneInfoSelectedEventArgs> sceneInfoSelector,
            ISelectedListener<SectionSelectedEventArgs> sectionSelector,
            ISelectedListener<TabSelectedEventArgs> tabSelector,
            IReaderSceneStarter sceneStarter,
            [Inject(Id = SaveType.Local)] IEncounterWriter encounterWriter)
        {
            SignalBus = signalBus;
            ConfirmationPopup = confirmationPopup;

            SceneInfoSelector = sceneInfoSelector;
            SectionSelector = sectionSelector;
            TabSelector = tabSelector;

            ReaderSceneStarter = sceneStarter;
            EncounterWriter = encounterWriter;
        }

        protected virtual Button Button { get; set; }
        protected virtual void Start()
        {
            Button = GetComponent<Button>();
            Button.interactable = false;

            SceneInfoSelector.Selected += SceneLoaded;
            if (SceneInfoSelector.CurrentValue != null)
                SceneLoaded(this, SceneInfoSelector.CurrentValue);
        }

        private void SceneLoaded(object sender, WriterSceneInfoSelectedEventArgs e)
        {
            Button.interactable = true;
            Button.onClick.AddListener(StartReader);
        }

        protected virtual void OnButtonClicked()
            => ConfirmationPopup.ShowConfirmation(SaveCase, StartReader, 
                "Save Changes", "Would you look to save your changes?", "Yes", "No");
        

        protected virtual void SaveCase()
        {
            SignalBus.Fire<SerializeEncounterSignal>();
            var sceneInfo = SceneInfoSelector.CurrentValue.SceneInfo;
            var parameters = new SaveEncounterParameters() {
                Encounter = sceneInfo.Encounter,
                User = sceneInfo.User,
                SaveVersion = SaveVersion.Private
            };
            EncounterWriter.Save(parameters);
            StartReader();
        }

        protected virtual void StartReader()
        {
            SceneInfoSelector.CurrentValue.SceneInfo.Encounter.Content.SetCurrentSection(SectionSelector.CurrentValue.SelectedSection);
            SectionSelector.CurrentValue.SelectedSection.SetCurrentTab(TabSelector.CurrentValue.SelectedTab);

            SignalBus.Fire<SerializeEncounterSignal>();
            var sceneInfo = SceneInfoSelector.CurrentValue.SceneInfo;
            var encounterStatus = new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus());
            var encounter = new UserEncounter(sceneInfo.User, sceneInfo.Encounter, encounterStatus);
            var encounterResult = new WaitableTask<UserEncounter>(encounter);
            var loadingInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounterResult);
            ReaderSceneStarter.StartScene(loadingInfo);
        }

    }
}