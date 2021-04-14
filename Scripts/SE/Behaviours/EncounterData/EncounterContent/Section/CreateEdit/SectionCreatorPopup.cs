
using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SectionCreatorPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button CreateButton { get => createButton; set => createButton = value; }
        [SerializeField] private Button createButton;

        public TMP_InputField NameField { get => nameField; set => nameField = value; }
        [SerializeField] private TMP_InputField nameField;
        public BaseColorEditor Color { get => color; set => color = value; }
        [SerializeField] private BaseColorEditor color;
        public IconSelectorUI IconSelector { get => iconSelector; set => iconSelector = value; }
        [SerializeField] private IconSelectorUI iconSelector;

        protected virtual string DefaultIconKey { get; } = "person";
        protected virtual Color DefaultColor { get; } = new Color(.9216f, .3012f, .3608f);

        protected BaseMessageHandler MessageHandler { get; set; }
        [Inject] public virtual void Inject(BaseMessageHandler messageHandler) => MessageHandler = messageHandler;

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            CreateButton.onClick.AddListener(AddSection);
        }

        protected WaitableTask<Section> CurrentWaitableSection { get; set; }
        public virtual WaitableTask<Section> CreateSection(ContentEncounter encounter)
        {
            CurrentWaitableSection?.SetError(new Exception("New popup opened"));
            CurrentWaitableSection = new WaitableTask<Section>();

            NameField.text = "";
            gameObject.SetActive(true);
            Color.Display(DefaultColor);
            IconSelector.Display(encounter, DefaultIconKey);

            return CurrentWaitableSection;
        }

        protected virtual void AddSection()
        {
            var name = NameField.text;
            if (string.IsNullOrEmpty(name)) {
                MessageHandler.ShowMessage("Name cannot be empty.", MessageType.Error);
                return;
            }
            var icon = IconSelector.Value;
            var color = Color.GetValue();

            var section = new Section(name, icon, color);
            CurrentWaitableSection.SetResult(section);
            CurrentWaitableSection = null;

            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableSection != null) {
                CurrentWaitableSection.SetError(new Exception("Canceled"));
                CurrentWaitableSection = null;
            }
            gameObject.SetActive(false);
        }
    }
}