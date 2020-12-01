using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileDialoguePanelsCreator : BaseChildUserPanelsDrawer
    {
        public Image Background { get => background; set => background = value; }
        [SerializeField] private Image background;
        public ColorType ChoiceBackgroundColor { get => choiceBackgroundColor; set => choiceBackgroundColor = value; }
        [SerializeField] private ColorType choiceBackgroundColor;
        public BaseReaderPanelBehaviour DialogueEntryLeft { get => dialogueEntryLeft; set => dialogueEntryLeft = value; }
        [SerializeField] private BaseReaderPanelBehaviour dialogueEntryLeft;
        public BaseReaderPanelBehaviour DialogueEntryRight { get => dialogueEntryRight; set => dialogueEntryRight = value; }
        [SerializeField] private BaseReaderPanelBehaviour dialogueEntryRight;
        public CompletableReaderPanelBehaviour DialogueChoice { get => dialogueChoice; set => dialogueChoice = value; }
        [SerializeField] private CompletableReaderPanelBehaviour dialogueChoice;

        protected IColorManager ColorManager { get; set; }
        protected BaseReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        [Inject]
        public virtual void Inject(IColorManager colorManager, BaseReaderPanelBehaviour.Factory readerPanelFactory)
        {
            ColorManager = colorManager;
            ReaderPanelFactory = readerPanelFactory;
        }

        public override void Display(OrderedCollection<UserPanel> panels, bool active)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            var panelBehaviours = new List<BaseReaderPanelBehaviour>();
            var childList = new List<UserPanel>(panels.Values);
            DeserializeChildren(panelBehaviours, childList, 0);
        }

        protected virtual void DeserializeChildren(List<BaseReaderPanelBehaviour> readerPanels, List<UserPanel> panels, int startIndex)
        {
            Background.color = Color.white;

            if (startIndex < readerPanels.Count)
                return;

            for (var i = startIndex; i < panels.Count; i++) {
                var panel = panels[i];
                if (!panel.Data.Type.Contains("DialogueEntry")) {
                    readerPanels.Add(CreateChoice(readerPanels, panels, i));
                    return;
                }

                readerPanels.Add(CreateEntry(panel));
            }
        }

        private const string CharacterNameKey = "characterName";
        private const string ProviderName = "Provider";
        protected virtual BaseReaderPanelBehaviour CreateEntry(UserPanel panel)
        {
            var values = panel.Data.Values;
            BaseReaderPanelBehaviour entryPrefab =
                (values.ContainsKey(CharacterNameKey) && values[CharacterNameKey] == ProviderName) ?
                DialogueEntryRight : DialogueEntryLeft;

            var panelDisplay = ReaderPanelFactory.Create(entryPrefab);
            panelDisplay.transform.SetParent(transform);
            panelDisplay.transform.localScale = Vector3.one;
            panelDisplay.transform.SetAsLastSibling();
            panelDisplay.Select(this, new UserPanelSelectedEventArgs(panel, true));
            return panelDisplay;
        }

        protected virtual CompletableReaderPanelBehaviour CreateChoice(List<BaseReaderPanelBehaviour> readerPanels, List<UserPanel> panels, int panelIndex)
        {
            Background.color = ColorManager.GetColor(ChoiceBackgroundColor);

            var panelDisplay = (CompletableReaderPanelBehaviour)ReaderPanelFactory.Create(DialogueChoice);
            panelDisplay.transform.SetParent(transform);
            panelDisplay.transform.localScale = Vector3.one;
            panelDisplay.transform.SetAsLastSibling();
            panelDisplay.Select(this, new UserPanelSelectedEventArgs(panels[panelIndex], true));

            panelDisplay.Completed += () => DeserializeChildren(readerPanels, panels, panelIndex + 1);
            return panelDisplay;
        }
    }
}