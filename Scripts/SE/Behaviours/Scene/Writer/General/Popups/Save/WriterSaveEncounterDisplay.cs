using ClinicalTools.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSaveEncounterDisplay : BaseSaveEncounterDisplay, ISelectedListener<EncounterMetadataSelectedEventArgs>, ICloseHandler
    {
        public Button SaveButton { get => saveButton; set => saveButton = value; }
        [SerializeField] private Button saveButton;

        public event SelectedHandler<EncounterMetadataSelectedEventArgs> Selected;
        public EncounterMetadataSelectedEventArgs CurrentValue { get; protected set; }

        protected SignalBus SignalBus { get; set; }
        protected BaseMessageHandler MessageHandler { get; set; }
        protected IEncounterWriter LocalWriter { get; set; }
        protected IEncounterWriter ServerWriter { get; set; }
        [Inject]
        public void Inject(
            SignalBus signalBus,
            BaseMessageHandler messageHandler,
            [Inject(Id = SaveType.Local)] IEncounterWriter localWriter,
            [Inject(Id = SaveType.Server)] IEncounterWriter serverWriter)
        {
            SignalBus = signalBus;
            MessageHandler = messageHandler;
            LocalWriter = localWriter;
            ServerWriter = serverWriter;
        }

        protected virtual void Awake()
        {
            SaveButton.onClick.AddListener(ServerSave);
        }
        protected ContentEncounter CurrentEncounter { get; set; }
        protected User CurrentUser { get; set; }


        public override void Display(User user, ContentEncounter encounter)
        {
            CurrentUser = user;
            gameObject.SetActive(true);

            CurrentEncounter = encounter;
            CurrentValue = new EncounterMetadataSelectedEventArgs(encounter.Metadata);
            Selected?.Invoke(this, CurrentValue);
        }

        protected virtual void Serialize() => SignalBus.Fire<SerializeEncounterSignal>();

        protected virtual void ServerSave()
        {
            Serialize();

            var saveParameters = new SaveEncounterParameters() {
                Encounter = CurrentEncounter,
                User = CurrentUser,
                SaveVersion = SaveVersion.Private
            };
            var savingResult = ServerWriter.Save(saveParameters);
            savingResult.AddOnCompletedListener((result)=>ServerSaveFinished(result, saveParameters));

            gameObject.SetActive(false);
        }

        protected virtual void ServerSaveFinished(TaskResult result, SaveEncounterParameters saveParameters)
        {
            if (!result.IsError())
                MessageHandler.ShowMessage("Successfully published case to the server.");
                // delete local save
                // delete autosave
            else { 
                MessageHandler.ShowMessage($"Could not publish case to server. Creating local save.\n{result.Exception.Message}", MessageType.Error);
                LocalWriter.Save(saveParameters);
            }
        }

        public void Close(object sender)
        {
            if (CurrentValue != null)
                Selected?.Invoke(this, CurrentValue);
            gameObject.SetActive(false);
        }
    }
}