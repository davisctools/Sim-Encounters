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
        [SerializeField] private GameObject completedObject;

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

            if (isOn) {
                text.color = Color.white;
                toggle.image.color = Section.Data.Color;
                completedObject.SetActive(false);
                outline.gameObject.SetActive(false);
                layoutGroup.padding.left = 30;
                layoutGroup.padding.right = 30;
            } else if (Section.IsRead()) {
                var colorManager = new ColorManager();
                text.color = colorManager.GetColor(ColorType.Gray5);
                toggle.image.color = Color.white;
                completedObject.SetActive(true);
                outline.gameObject.SetActive(true);
                layoutGroup.padding.left = 30;
                layoutGroup.padding.right = 10;
            } else {
                var colorManager = new ColorManager();
                text.color = Color.white;
                toggle.image.color = colorManager.GetColor(ColorType.Gray5);
                completedObject.SetActive(false);
                outline.gameObject.SetActive(false);
                layoutGroup.padding.left = 30;
                layoutGroup.padding.right = 30;
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