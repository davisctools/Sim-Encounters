using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterPublisher
    {
        WaitableTask<ContentEncounter> Publish(User user, WaitableTask<ContentEncounter> encounter);
    }

    public class EncounterPublisher : IEncounterPublisher
    {
        protected IEncounterWriter ServerWriter { get; set; }
        [Inject]
        public void Inject([Inject(Id = SaveType.Server)] IEncounterWriter serverWriter) => ServerWriter = serverWriter;

        public virtual WaitableTask<ContentEncounter> Publish(User user, WaitableTask<ContentEncounter> encounter)
        {
            var task = new WaitableTask<ContentEncounter>();
            encounter.AddOnCompletedListener((result) => OnEncounterRetrieved(task, user, result));

            return task;
        }

        protected virtual void OnEncounterRetrieved(WaitableTask<ContentEncounter> encounterTask, User user, TaskResult<ContentEncounter> encounterResult)
        {
            var parameters = new SaveEncounterParameters() {
                Encounter = encounterResult.Value,
                User = user
            };
            var publishTask = ServerWriter.Save(parameters);
            publishTask.AddOnCompletedListener((result) => OnPublished(encounterTask, encounterResult.Value, result));
        }

        protected virtual void OnPublished(WaitableTask<ContentEncounter> encounterTask, ContentEncounter encounter, TaskResult result)
        {
            encounter.Metadata.Filename = encounter.Metadata.GetDesiredFilename();
            encounterTask.SetResult(encounter);
        }
    }

    public interface IEncounterCreator
    {
        WaitableTask<ContentEncounter> CreateEncounter(User user, OldEncounterMetadata metadata, WaitableTask<EncounterContentData> encounterData);
    }
    public class EncounterCreator : IEncounterCreator
    {
        protected IEncounterPublisher EncounterPublisher { get; set; }
        [Inject]
        protected virtual void Inject(IEncounterPublisher encounterPublisher) => EncounterPublisher = encounterPublisher;

        public virtual WaitableTask<ContentEncounter> CreateEncounter(User user, OldEncounterMetadata metadata, WaitableTask<EncounterContentData> encounterData)
            => EncounterPublisher.Publish(user, CreateEncounterFromData(metadata, encounterData));

        protected virtual WaitableTask<ContentEncounter> CreateEncounterFromData(OldEncounterMetadata metadata, WaitableTask<EncounterContentData> encounterData)
        {
            var task = new WaitableTask<ContentEncounter>();
            encounterData.AddOnCompletedListener((result) => OnDataRetrieved(task, metadata, result));
            return task;
        }

        protected virtual void OnDataRetrieved(WaitableTask<ContentEncounter> encounterTask, OldEncounterMetadata metadata, TaskResult<EncounterContentData> encounterData)
        {
            var encounter = new ContentEncounter(metadata, encounterData.Value);
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
        protected OldEncounterMetadata CurrentMetadata { get; set; }
        protected WaitableTask<EncounterContentData> EncounterData { get; set; }
        public override void Display(MenuSceneInfo sceneInfo, MenuEncounter encounter)
        {
            SceneInfo = sceneInfo;
            var metadata = encounter.GetLatestTypedMetada();
            CurrentMetadata = new OldEncounterMetadata(metadata.Value);
            SetFields();
            var dataReader = DataReaderSelector.GetEncounterDataReader(metadata.Key);
            EncounterData = dataReader.GetEncounterData(sceneInfo.User, metadata.Value);
        }

        public override void Display(MenuSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
            CurrentMetadata = new OldEncounterMetadata();
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