using ClinicalTools.SEColors;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionToggle : BaseSelectableUserSectionBehaviour
    {
        [SerializeField] private LayoutGroup layoutGroup;
        [SerializeField] private GameObject outline;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Toggle toggle;

        protected virtual void Awake()
        {
            toggle.onValueChanged.AddListener(ToggleChanged);
        }
        public override void Initialize(UserSection section)
        {
            base.Initialize(section);
            ToggleChanged(false);
        }

        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserEncounterSelectedEventArgs> encounterSelectedListener)
            => EncounterSelectedListener = encounterSelectedListener;

        protected virtual void ToggleChanged(bool isOn)
        {
            if (isOn && EncounterSelectedListener.CurrentValue.Encounter.GetCurrentSection() != Section)
                Select(this, new UserTabSelectedEventArgs(Section.Tabs[0].Value, ChangeType.JumpTo));

            toggle.interactable = !isOn;
            if (isOn) {
                text.color = Color.white;
                toggle.image.color = Section.Data.Color;
                outline.gameObject.SetActive(false);
            } else if (Section.IsRead()) {
                var colorManager = new ColorManager();
                text.color = colorManager.GetColor(ColorType.Gray5);
                toggle.image.color = Color.white;
                outline.gameObject.SetActive(true);
            } else {
                var colorManager = new ColorManager();
                text.color = Color.white;
                toggle.image.color = colorManager.GetColor(ColorType.Gray5);
                outline.gameObject.SetActive(false);
            }
        }

        public virtual void SetToggleGroup(ToggleGroup toggleGroup)
        {
            toggle.group = toggleGroup;
        }

        public virtual void Select()
        {
            toggle.isOn = true;
        }
    }
}