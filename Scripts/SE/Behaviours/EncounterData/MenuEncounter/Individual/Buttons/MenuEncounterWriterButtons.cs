using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncounterWriterButtons : MonoBehaviour
    {
        public virtual Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public virtual Button CopyButton { get => copyButton; set => copyButton = value; }
        [SerializeField] private Button copyButton;
        public virtual bool IsTemplate { get => isTemplate; set => isTemplate = value; }
        [SerializeField] private bool isTemplate;

        protected IMenuEncounterStarter EncounterStarter { get; set; }
        protected BaseAddEncounterPopup AddEncounterPopup { get; set; }
        protected virtual ISelectedListener<MenuSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected virtual ISelectedListener<MenuEncounterSelectedEventArgs> MenuEncounterSelectedListener { get; set; }
        [Inject] protected virtual void Inject(
            IMenuEncounterStarter encounterStarter,
            ISelectedListener<MenuSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            ISelectedListener<MenuEncounterSelectedEventArgs> menuEncounterSelectedListener,
            BaseAddEncounterPopup addEncounterPopup) {
            EncounterStarter = encounterStarter;
            AddEncounterPopup = addEncounterPopup;
            SceneInfoSelectedListener = sceneInfoSelectedListener;

            MenuEncounterSelectedListener = menuEncounterSelectedListener;
            MenuEncounterSelectedListener.Selected += MenuEncounterSelected;
            if (MenuEncounterSelectedListener.CurrentValue != null)
                MenuEncounterSelected(MenuEncounterSelectedListener, MenuEncounterSelectedListener.CurrentValue);
        }

        protected virtual void Awake()
        {
            if (EditButton != null)
                EditButton.onClick.AddListener(StartEncounter);
            if (CopyButton != null)
                CopyButton.onClick.AddListener(CopyEncounter);
        }

        protected MenuSceneInfo SceneInfo => SceneInfoSelectedListener.CurrentValue.SceneInfo;
        protected MenuEncounter MenuEncounter => MenuEncounterSelectedListener.CurrentValue.Encounter;

        protected virtual void MenuEncounterSelected(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            var metadata = eventArgs.Encounter.GetLatestMetadata();
            if (eventArgs.SelectionType != EncounterSelectionType.Edit || IsTemplate != metadata.IsTemplate) {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            if (EditButton != null)
                EditButton.gameObject.SetActive(metadata.AuthorAccountId == SceneInfo.User.AccountId && eventArgs.Encounter.Lock == null);
            if (CopyButton != null)
                CopyButton.gameObject.SetActive(true);
        }

        public virtual void StartEncounter() => EncounterStarter.StartEncounter(SceneInfo, MenuEncounter);
        public virtual void CopyEncounter() => AddEncounterPopup.Display(SceneInfo, MenuEncounter);
    }
}