using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuEncounterUI : MenuEncounterSelector
    {
        public virtual Button SelectButton { get => selectButton; set => selectButton = value; }
        [SerializeField] private Button selectButton;
        public virtual GameObject InProgressObject { get => inProgressObject; set => inProgressObject = value; }
        [SerializeField] private GameObject inProgressObject;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }
        [SerializeField] private GameObject completedObject;

        protected virtual BaseMenuEncounterOverview MenuEncounterOverview { get; set; }
        [Inject] public virtual void Inject(BaseMenuEncounterOverview menuEncounterOverview) => MenuEncounterOverview = menuEncounterOverview;

        protected virtual void Start() => SelectButton.onClick.AddListener(OnSelected);
        protected virtual void OnSelected()
        {
            Select(this, CurrentValue);
            MenuEncounterOverview.Select(this, CurrentValue);
        }
        public override void Select(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);

            var status = eventArgs.Encounter.Status;
            CompletedObject.SetActive(status?.Completed == true);
            InProgressObject.SetActive(status?.Completed == false);
        }

        // The proper way to remove this after being deleted is to despawn it
        // This is easier codewise with little overhead, but still not optimal
        protected virtual void Update()
        {
            if (CurrentValue?.Encounter.Metadata.Count == 0)
                gameObject.SetActive(false);
        }
    }
}