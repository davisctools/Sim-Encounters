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
        protected ISelectedListener<ReaderSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }

        [Inject]
        public void Inject(
            IWriterSceneStarter writerSceneStarter,
            ISelectedListener<ReaderSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelectedListener)
        {
            WriterSceneStarter = writerSceneStarter;
            SceneInfoSelectedListener = sceneInfoSelectedListener;

            EncounterSelectedListener = encounterSelectedListener;
            EncounterSelectedListener.Selected += OnEncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                OnEncounterSelected(EncounterSelectedListener, EncounterSelectedListener.CurrentValue);
        }

        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs e)
            => gameObject.SetActive(e.Encounter.Data.Metadata.AuthorAccountId == e.Encounter.User.AccountId);

        protected virtual void Awake() => Button.onClick.AddListener(ShowInstructions);
        public virtual void ShowInstructions()
        {
            var sceneInfo = SceneInfoSelectedListener.CurrentValue.SceneInfo;
            var encounter = new WaitableTask<Encounter>(sceneInfo.Encounter.Data);
            var writerSceneInfo = new LoadingWriterSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            WriterSceneStarter.StartScene(writerSceneInfo);
        }
    }
}