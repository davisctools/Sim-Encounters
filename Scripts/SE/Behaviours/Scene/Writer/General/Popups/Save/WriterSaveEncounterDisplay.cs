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
        protected IEncounterWriter LocalWriter { get; set; }
        protected IEncounterWriter ServerWriter { get; set; }
        [Inject]
        public void Inject(
            SignalBus signalBus,
            [Inject(Id = SaveType.Local)] IEncounterWriter localWriter,
            [Inject(Id = SaveType.Server)] IEncounterWriter serverWriter)
        {
            SignalBus = signalBus;
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

        protected virtual void Serialize()
        {
            SignalBus.Fire<SerializeEncounterSignal>();
        }
        protected virtual void Save()
        {
            Serialize();
            LocalWriter.Save(CurrentUser, CurrentEncounter);

            gameObject.SetActive(false);
        }

        protected virtual void Publish()
        {
            Serialize();
            var savingResult = ServerWriter.Save(CurrentUser, CurrentEncounter);
            savingResult.AddOnCompletedListener(PublishingFinished);

            gameObject.SetActive(false);
        }

        protected virtual void PublishingFinished(TaskResult result) => LocalWriter.Save(CurrentUser, CurrentEncounter);

        public void Close(object sender)
        {
            if (CurrentValue != null)
                Selected?.Invoke(this, CurrentValue);
            gameObject.SetActive(false);
        }
    }
}