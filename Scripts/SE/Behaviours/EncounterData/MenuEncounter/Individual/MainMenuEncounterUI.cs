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
    }
}