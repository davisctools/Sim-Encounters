using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class DeleteEncounterButtonGroup : MonoBehaviour
    {
        protected Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;

        protected IDeleteEncounterHandler DeleteEncounterHandler { get; set; }
        protected ISelectedListener<MenuSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected ISelectedListener<MenuEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected BaseMenuEncounterOverview EncounterOverview { get; set; }
        [Inject]
        public virtual void Inject(
            IDeleteEncounterHandler deleteEncounterHandler,
            ISelectedListener<MenuSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            ISelectedListener<MenuEncounterSelectedEventArgs> encounterSelectedListener,
            BaseMenuEncounterOverview encounterOverview)
        {
            DeleteEncounterHandler = deleteEncounterHandler;
            SceneInfoSelectedListener = sceneInfoSelectedListener;
            EncounterSelectedListener = encounterSelectedListener;
            EncounterOverview = encounterOverview;
        }

        protected User User => SceneInfoSelectedListener.CurrentValue.SceneInfo.User;


        protected virtual void Start()
        {
            Button.onClick.AddListener(Delete);
            EncounterSelectedListener.Selected += EncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                EncounterSelected(this, EncounterSelectedListener.CurrentValue);
        }

        protected MenuEncounter MenuEncounter { get; set; }
        protected bool CanDelete { get; set; }
        protected virtual void EncounterSelected(object sender, MenuEncounterSelectedEventArgs e)
        {
            MenuEncounter = e.Encounter;
            var authorAccountId = MenuEncounter.GetLatestMetadata().AuthorAccountId;
            CanDelete = MenuEncounter.Metadata.ContainsKey(SaveType.Local) ||
                (MenuEncounter.Metadata.ContainsKey(SaveType.Server) && authorAccountId == User.AccountId);
            gameObject.SetActive(CanDelete);
        }

        protected virtual void Delete()
        {
            if (!CanDelete)
                return;

            var deleteTask = DeleteEncounterHandler.Delete(User, EncounterSelectedListener.CurrentValue.Encounter);
            deleteTask.AddOnCompletedListener(EncounterDeleted);
        }

        protected virtual void EncounterDeleted(TaskResult result)
        {
            if (!result.IsError())
                EncounterOverview.Hide();
        }
    }
}