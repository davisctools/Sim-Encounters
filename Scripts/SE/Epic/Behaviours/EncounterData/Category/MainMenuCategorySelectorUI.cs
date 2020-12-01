using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuCategorySelectorUI : CategorySelector
    {
        public TextMeshProUGUI CategoryLabel { get => categoryLabel; set => categoryLabel = value; }
        [SerializeField] private TextMeshProUGUI categoryLabel;
        public Button SelectButton { get => selectButton; set => selectButton = value; }
        [SerializeField] private Button selectButton;

        public virtual GameObject InProgressObject { get => inProgressObject; set => inProgressObject = value; }
        [SerializeField] private GameObject inProgressObject;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }
        [SerializeField] private GameObject completedObject;

        protected virtual void Start() => SelectButton.onClick.AddListener(CategorySelected);

        public override void Select(object sender, CategorySelectedEventArgs eventArgs)
        {
            CategoryLabel.text = eventArgs.Category.Name;

            if (eventArgs.Category.IsCompleted())
                CompletedObject.SetActive(true);

            base.Select(sender, eventArgs);
        }

        protected virtual void CategorySelected() => Select(this, CurrentValue);

    }
    public abstract class CategorySelector : MonoBehaviour,
        ISelector<CategorySelectedEventArgs>
    {
        public virtual CategorySelectedEventArgs CurrentValue { get; protected set; }

        public event SelectedHandler<CategorySelectedEventArgs> Selected;

        public virtual void Select(object sender, CategorySelectedEventArgs eventArgs)
        {
            CurrentValue = eventArgs;
            Selected?.Invoke(sender, eventArgs);
        }

        public class Pool : SceneMonoMemoryPool<CategorySelector>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}