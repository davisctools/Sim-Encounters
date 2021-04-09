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
        public Button PublishButton { get => publishButton; set => publishButton = value; }
        [SerializeField] private Button publishButton;

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
            [Inject(Id = SaveType.Server)] IEncounterWriter localWriter,
            [Inject(Id = SaveType.Server)] IEncounterWriter serverWriter)
        {
            SignalBus = signalBus;
            MessageHandler = messageHandler;
            LocalWriter = localWriter;
            ServerWriter = serverWriter;
        }

        protected virtual void Awake()
        {
            SaveButton.onClick.AddListener(Save);
            PublishButton.onClick.AddListener(Publish);
        }
        protected Encounter CurrentEncounter { get; set; }
        protected User CurrentUser { get; set; }


        public override void Display(User user, Encounter encounter)
        {
            CurrentUser = user;
            gameObject.SetActive(true);

            CurrentEncounter = encounter;
            CurrentValue = new EncounterMetadataSelectedEventArgs(encounter.Metadata);
            Selected?.Invoke(this, CurrentValue);
        }

        protected virtual void Serialize() => SignalBus.Fire<SerializeEncounterSignal>();

        protected virtual void Save()
        {
            Serialize();

            var parameters = new SaveEncounterParameters() {
                Encounter = CurrentEncounter,
                User = CurrentUser,
                SaveVersion = SaveVersion.Private
            };
            LocalWriter.Save(parameters);

            gameObject.SetActive(false);
        }

        protected virtual void Publish()
        {
            Serialize();

            var parameters = new SaveEncounterParameters() {
                Encounter = CurrentEncounter,
                User = CurrentUser,
                SaveVersion = SaveVersion.Public
            };
            var savingResult = ServerWriter.Save(parameters);
            savingResult.AddOnCompletedListener(PublishingFinished);

            gameObject.SetActive(false);
        }

        protected virtual void PublishingFinished(TaskResult result)
        {
            var parameters = new SaveEncounterParameters() {
                Encounter = CurrentEncounter,
                User = CurrentUser,
                SaveVersion = SaveVersion.Public
            };
            LocalWriter.Save(parameters);
            if (!result.IsError())
                MessageHandler.ShowMessage("Successfully published case to the server.");
            else
                MessageHandler.ShowMessage($"Could not publish case to server.\n{result.Exception.Message}", MessageType.Error);
        }

        public void Close(object sender)
        {
            if (CurrentValue != null)
                Selected?.Invoke(this, CurrentValue);
            gameObject.SetActive(false);
        }
    }
}