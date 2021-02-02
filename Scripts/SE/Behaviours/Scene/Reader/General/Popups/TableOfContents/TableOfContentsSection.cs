using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TableOfContentsSection : BaseTableOfContentsSection
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image image;
        [SerializeField] private Sprite onImage;
        private Sprite offImage;

        protected virtual void Awake()
        {
            offImage = image.sprite;
            toggle.onValueChanged.AddListener(ToggleChanged);
        }

        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        [Inject] public virtual void Inject(ISelectedListener<UserEncounterSelectedEventArgs> encounterSelectedListener)
            => EncounterSelectedListener = encounterSelectedListener;

        protected virtual void ToggleChanged(bool isOn)
            => image.sprite = isOn ? onImage : offImage;

        private bool started = false;
        protected virtual void Start()
        {
            ResetIsOn();
            started = true;
        }

        public override void Initialize(UserSection section)
        {
            base.Initialize(section);
            if (started)
                ResetIsOn();
        }

        protected virtual void OnEnable() => ResetIsOn();

        protected virtual void ResetIsOn()
        {
            if (Section == null)
                return;
            toggle.isOn = EncounterSelectedListener.CurrentValue.Encounter.GetCurrentSection() == Section;
        }
    }
}