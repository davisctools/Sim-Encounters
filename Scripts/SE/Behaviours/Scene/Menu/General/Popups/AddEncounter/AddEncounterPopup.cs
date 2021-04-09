using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterPublisher
    {
        WaitableTask<Encounter> Publish(User user, WaitableTask<Encounter> encounter);
    }

    public class EncounterPublisher : IEncounterPublisher
    {
        protected IEncounterWriter ServerWriter { get; set; }
        [Inject]
        public void Inject([Inject(Id = SaveType.Server)] IEncounterWriter serverWriter) => ServerWriter = serverWriter;

        public virtual WaitableTask<Encounter> Publish(User user, WaitableTask<Encounter> encounter)
        {
            var task = new WaitableTask<Encounter>();
            encounter.AddOnCompletedListener((result) => OnEncounterRetrieved(task, user, result));

            return task;
        }

        protected virtual void OnEncounterRetrieved(WaitableTask<Encounter> encounterTask, User user, TaskResult<Encounter> encounterResult)
        {
            var parameters = new SaveEncounterParameters() {
                Encounter = encounterResult.Value,
                User = user
            };
            var publishTask = ServerWriter.Save(parameters);
            publishTask.AddOnCompletedListener((result) => OnPublished(encounterTask, encounterResult.Value, result));
        }

        protected virtual void OnPublished(WaitableTask<Encounter> encounterTask, Encounter encounter, TaskResult result)
        {
            encounter.Metadata.Filename = encounter.Metadata.GetDesiredFilename();
            encounterTask.SetResult(encounter);
        }
    }

    public interface IEncounterCreator
    {
        WaitableTask<Encounter> CreateEncounter(User user, EncounterMetadata metadata, WaitableTask<EncounterContent> encounterData);
    }
    public class EncounterCreator : IEncounterCreator
    {
        protected IEncounterPublisher EncounterPublisher { get; set; }
        [Inject]
        protected virtual void Inject(IEncounterPublisher encounterPublisher) => EncounterPublisher = encounterPublisher;

        public virtual WaitableTask<Encounter> CreateEncounter(User user, EncounterMetadata metadata, WaitableTask<EncounterContent> encounterData)
            => EncounterPublisher.Publish(user, CreateEncounterFromData(metadata, encounterData));

        protected virtual WaitableTask<Encounter> CreateEncounterFromData(EncounterMetadata metadata, WaitableTask<EncounterContent> encounterData)
        {
            var task = new WaitableTask<Encounter>();
            encounterData.AddOnCompletedListener((result) => OnDataRetrieved(task, metadata, result));
            return task;
        }

        protected virtual void OnDataRetrieved(WaitableTask<Encounter> encounterTask, EncounterMetadata metadata, TaskResult<EncounterContent> encounterData)
        {
            var encounter = new Encounter(metadata, encounterData.Value);
            encounterTask.SetResult(encounter);
        }
    }

    public class AddEncounterPopup : BaseAddEncounterPopup
    {
        public TMP_InputField TitleField { get => titleField; set => titleField = value; }
        [SerializeField] private TMP_InputField titleField;
        public TMP_InputField DescriptionField { get => descriptionField; set => descriptionField = value; }
        [SerializeField] private TMP_InputField descriptionField;

        public virtual Button StartCaseButton { get => startCaseButton; set => startCaseButton = value; }
        [SerializeField] private Button startCaseButton;
        public virtual Button CloseButton { get => closeButton; set => closeButton = value; }
        [SerializeField] private Button closeButton;

        protected IEncounterDataReaderSelector DataReaderSelector { get; set; }
        protected IWriterSceneStarter SceneStarter { get; set; }
        protected IEncounterCreator EncounterCreator { get; set; }
        [Inject]
        protected virtual void Inject(IEncounterDataReaderSelector dataReaderSelector, IWriterSceneStarter sceneStarter, IEncounterCreator encounterCreator)
        {
            DataReaderSelector = dataReaderSelector;
            SceneStarter = sceneStarter;
            EncounterCreator = encounterCreator;
        }

        protected virtual void Awake()
        {
            StartCaseButton.onClick.AddListener(StartCase);
            CloseButton.onClick.AddListener(Close);
        }

        protected MenuSceneInfo SceneInfo { get; set; }
        protected EncounterMetadata CurrentMetadata { get; set; }
        protected WaitableTask<EncounterContent> EncounterData { get; set; }
        public override void Display(MenuSceneInfo sceneInfo, MenuEncounter encounter)
        {
            SceneInfo = sceneInfo;
            var metadata = encounter.GetLatestTypedMetada();
            CurrentMetadata = new EncounterMetadata(metadata.Value);
            SetFields();
            var dataReader = DataReaderSelector.GetEncounterDataReader(metadata.Key);
            EncounterData = dataReader.GetEncounterData(sceneInfo.User, metadata.Value);
        }

        public override void Display(MenuSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
            CurrentMetadata = new EncounterMetadata();
            SetFields();
            var dataReader = DataReaderSelector.GetEncounterDataReader(SaveType.Default);
            EncounterData = dataReader.GetEncounterData(sceneInfo.User, CurrentMetadata);
        }

        protected virtual void SetFields()
        {
            gameObject.SetActive(true);
            TitleField.text = "";
            DescriptionField.text = "";
        }

        protected virtual void StartCase()
        {
            CurrentMetadata.Author = new Author(SceneInfo.User.AccountId) { Name = SceneInfo.User.Name };
            CurrentMetadata.Title = TitleField.text;
            CurrentMetadata.Description = DescriptionField.text;

            var rand = new System.Random((int)DateTime.UtcNow.Ticks);
            CurrentMetadata.RecordNumber = -rand.Next(100000, 1000000);
            CurrentMetadata.Filename = CurrentMetadata.GetDesiredFilename();

            var encounter = EncounterCreator.CreateEncounter(SceneInfo.User, CurrentMetadata, EncounterData);

            var writerInfo = new LoadingWriterSceneInfo(SceneInfo.User, SceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(writerInfo);
        }

        protected virtual void Close() => gameObject.SetActive(false);
    }
}