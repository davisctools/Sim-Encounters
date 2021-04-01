using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
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
        [Inject]
        protected virtual void Inject(IEncounterDataReaderSelector dataReaderSelector, IWriterSceneStarter sceneStarter)
        {
            DataReaderSelector = dataReaderSelector;
            SceneStarter = sceneStarter;
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
            CurrentMetadata.Author = new Author(SceneInfo.User.AccountId) {
                Name = SceneInfo.User.Name
            };
            CurrentMetadata.Title = TitleField.text;
            CurrentMetadata.Description = DescriptionField.text;
            var rand = new System.Random((int)DateTime.UtcNow.Ticks);
            CurrentMetadata.RecordNumber = -rand.Next(100000, 1000000);
            CurrentMetadata.Filename = $"{CurrentMetadata.RecordNumber}{CurrentMetadata.Title}";
            var encounter = new WaitableTask<Encounter>();
            var writerInfo = new LoadingWriterSceneInfo(SceneInfo.User, SceneInfo.LoadingScreen, encounter);
            EncounterData.AddOnCompletedListener((result) => CreateEncounter(encounter, result));
            SceneStarter.StartScene(writerInfo);
        }

        protected virtual void CreateEncounter(WaitableTask<Encounter> encounter, TaskResult<EncounterContent> encounterData)
        {
            var result = new Encounter(CurrentMetadata, encounterData.Value);
            encounter.SetResult(result);
        }

        protected virtual void Close() => gameObject.SetActive(false);
    }
}