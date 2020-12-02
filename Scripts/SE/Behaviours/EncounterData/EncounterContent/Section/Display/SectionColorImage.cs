using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class SectionColorImage : MonoBehaviour
    {
        private Image image;
        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected ISelectedListener<SectionSelectedEventArgs> SectionSelector { get; set; }
        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<SectionSelectedEventArgs> sectionSelector)
        {
            SectionSelector = sectionSelector;
        }
        protected virtual void Start()
        {
            SectionSelector.Selected += OnSectionSelected;
            if (SectionSelector.CurrentValue != null)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);
        }

        protected virtual void OnSectionSelected(object sender, SectionSelectedEventArgs eventArgs)
            => Image.color = eventArgs.SelectedSection.Color;
    }
}