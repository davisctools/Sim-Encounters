using ClinicalTools.Collections;
using ClinicalTools.SEColors;
using System;
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
        public BaseReaderPanelBehaviour TextboxPrefab { get => textboxPrefab; set => textboxPrefab = value; }
        [SerializeField] private BaseReaderPanelBehaviour textboxPrefab;

        protected virtual IColorManager ColorManager { get; } = new ColorManager();
        protected BaseReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        [Inject]
        public virtual void Inject(BaseReaderPanelBehaviour.Factory readerPanelFactory) => ReaderPanelFactory = readerPanelFactory;


        protected bool WasActive { get; set; }

        protected List<BaseReaderPanelBehaviour> PanelBehaviours {get; } = new List<BaseReaderPanelBehaviour>();
        protected OrderedCollection<UserPanel> Panels { get; set; }
        public override void Display(OrderedCollection<UserPanel> panels, bool active)
        {
            if (active) {
                foreach (var panel in panels)
                    panel.Value.SetRead(true);
            }

            if (!WasActive && active && Panels == panels) {
                WasActive = active;

                for (var i = 0; i < PanelBehaviours.Count; i++)
                    PanelBehaviours[i].Select(this, new UserPanelSelectedEventArgs(Panels[i].Value, WasActive));

                return;
            }

            WasActive = active;
            Panels = panels;

            foreach (Transform child in transform)
                Destroy(child.gameObject);

            PanelBehaviours.Clear();
            var childList = new List<UserPanel>(panels.Values);
            DeserializeChildren(childList, 0);
        }

        protected virtual void DeserializeChildren(List<UserPanel> panels, int startIndex)
        {
            Background.color = Color.white;

            if (startIndex < PanelBehaviours.Count)
                return;

            for (var i = startIndex; i < panels.Count; i++) {
                var panel = panels[i];
                if (IsDialogueChoice(panel.Data.Type)) {
                    PanelBehaviours.Add(CreateChoice(panels, i));
                    return;
                }

                PanelBehaviours.Add(CreateEntry(panel));
            }
        }

        protected virtual bool IsDialogueChoice(string panelType)
            => !panelType.Equals("DialogueTextbox", StringComparison.InvariantCultureIgnoreCase) 
            && !panelType.Contains("DialogueEntry");

        protected virtual BaseReaderPanelBehaviour CreateEntry(UserPanel panel)
        {
            var entryPrefab = GetDialogueEntryPrefab(panel);
            var panelDisplay = ReaderPanelFactory.Create(entryPrefab);
            panelDisplay.transform.SetParent(transform);
            panelDisplay.transform.localScale = Vector3.one;
            panelDisplay.transform.SetAsLastSibling();
            panelDisplay.Select(this, new UserPanelSelectedEventArgs(panel, WasActive));
            return panelDisplay;
        }

        private const string DirectionKey = "direction";
        private const string RightValue = "Right";
        private const string CharacterNameKey = "characterName";
        private const string ProviderName = "Provider";
        protected virtual BaseReaderPanelBehaviour GetDialogueEntryPrefab(UserPanel panel)
        {
            if (panel.Data.Type.Equals("DialogueTextbox", StringComparison.InvariantCultureIgnoreCase))
                return TextboxPrefab;

            var values = panel.Data.LegacyValues;
            if (values.ContainsKey(DirectionKey))
                return values[DirectionKey].Equals(RightValue, StringComparison.InvariantCultureIgnoreCase) ? DialogueEntryRight : DialogueEntryLeft;

            return (values.ContainsKey(CharacterNameKey) && values[CharacterNameKey] == ProviderName) ? DialogueEntryRight : DialogueEntryLeft;
        }

        protected virtual CompletableReaderPanelBehaviour CreateChoice(List<UserPanel> panels, int panelIndex)
        {
            Background.color = ColorManager.GetColor(ChoiceBackgroundColor);

            var panelDisplay = (CompletableReaderPanelBehaviour)ReaderPanelFactory.Create(DialogueChoice);
            panelDisplay.transform.SetParent(transform);
            panelDisplay.transform.localScale = Vector3.one;
            panelDisplay.transform.SetAsLastSibling();
            panelDisplay.Select(this, new UserPanelSelectedEventArgs(panels[panelIndex], WasActive));

            panelDisplay.Completed += () => DeserializeChildren(panels, panelIndex + 1);
            return panelDisplay;
        }
    }
}