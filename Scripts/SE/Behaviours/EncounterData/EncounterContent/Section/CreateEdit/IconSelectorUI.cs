
using ClinicalTools.UI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class IconSelectorUI : MonoBehaviour
    {
        public Transform IconsParent { get => iconsParent; set => iconsParent = value; }
        [SerializeField] private Transform iconsParent;

        public string Value => GetIconReference(SelectedIconToggle);

        protected Toggle[] IconToggles { get; set; }
        protected Toggle SelectedIconToggle { get; set; }
        protected virtual void Awake()
        {
            IconToggles = IconsParent.GetComponentsInChildren<Toggle>();
            foreach (var iconToggle in IconToggles)
                iconToggle.AddOnSelectListener(() => SelectedIconToggle = iconToggle);

            SelectedIconToggle = IconToggles[0];
        }

        public virtual void Display(ContentEncounter encounter, string iconKey)
        {
            foreach (var iconToggle in IconToggles) {
                if (iconKey != GetIconReference(iconToggle))
                    continue;

                SelectedIconToggle = iconToggle;
                SelectedIconToggle.isOn = true;
                break;
            }
        }

        protected virtual string IconName => "Icon";
        /**
         * Gets the icon image based on the selected section icon.
         */
        protected virtual string GetIconReference(Toggle iconToggle)
        {
            if (iconToggle == null)
                return null;

            return iconToggle.transform.Find(IconName).GetComponent<Image>().sprite.name;
        }
    }
}