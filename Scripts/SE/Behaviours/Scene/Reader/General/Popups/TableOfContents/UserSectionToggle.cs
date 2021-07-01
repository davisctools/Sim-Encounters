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

        [SerializeField] private Image check;
        [SerializeField] private Image checkBackground;

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

                if (check != null)
                    check.color = Section.Data.Color;
                if (checkBackground != null)
                    checkBackground.color = Color.white;
            } else if (Section.IsRead()) {
                var colorManager = new ColorManager();
                var grayColor = colorManager.GetColor(ColorType.Gray5);
                text.color = grayColor;
                toggle.image.color = Color.white;
                outline.gameObject.SetActive(true);

                if (check != null)
                    check.color = Color.white;
                if (checkBackground != null)
                    checkBackground.color = grayColor;
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