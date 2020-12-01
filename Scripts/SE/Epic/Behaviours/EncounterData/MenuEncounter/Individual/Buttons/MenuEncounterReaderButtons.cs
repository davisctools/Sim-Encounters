using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncounterReaderButtons : MonoBehaviour
    {
        public virtual Button ReadButton { get => readButton; set => readButton = value; }
        [SerializeField] private Button readButton;
        public virtual TextMeshProUGUI ReadText { get => readText; set => readText = value; }
        [SerializeField] private TextMeshProUGUI readText;
        public virtual bool UseCaps { get => useCaps; set => useCaps = value; }
        [SerializeField] private bool useCaps;

        protected IMenuEncounterStarter EncounterStarter { get; set; }
        protected virtual ISelectedListener<MenuSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected virtual ISelectedListener<MenuEncounterSelectedEventArgs> MenuEncounterSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            IMenuEncounterStarter encounterStarter,
            ISelectedListener<MenuSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            ISelectedListener<MenuEncounterSelectedEventArgs> menuEncounterSelectedListener)
        {
            EncounterStarter = encounterStarter;
            SceneInfoSelectedListener = sceneInfoSelectedListener;

            MenuEncounterSelectedListener = menuEncounterSelectedListener;
            MenuEncounterSelectedListener.Selected += MenuEncounterSelected;
            if (MenuEncounterSelectedListener.CurrentValue != null)
                MenuEncounterSelected(MenuEncounterSelectedListener, MenuEncounterSelectedListener.CurrentValue);
        }

        protected virtual void Awake() => ReadButton.onClick.AddListener(StartEncounter);

        protected virtual void MenuEncounterSelected(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            if (eventArgs.SelectionType != EncounterSelectionType.Read) {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            ReadButton.gameObject.SetActive(true);
            var readText = GetReadButtonText(eventArgs.Encounter.Status);
            if (UseCaps)
                readText = readText.ToUpper();
            ReadText.text = readText;
        }

        protected virtual string GetReadButtonText(EncounterBasicStatus basicStatus)
        {
            if (basicStatus == null)
                return "Start Case";
            else if (basicStatus.Completed)
                return "Review Case";
            else
                return "Continue Case";
        }

        public virtual void StartEncounter()
        {
            EncounterStarter.StartEncounter(SceneInfoSelectedListener.CurrentValue.SceneInfo, MenuEncounterSelectedListener.CurrentValue.Encounter);
        }            
    }
}