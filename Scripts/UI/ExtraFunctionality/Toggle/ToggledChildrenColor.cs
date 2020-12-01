using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(ToggledChildrenColor))]
    public class ToggledChildrenColor : MonoBehaviour
    {
        [SerializeField] private Color color = Color.white;
        [SerializeField] private GameObject toggleParent = null;

        private Color unselectedColor;
        private Image img;
        private Toggle[] toggles;

        private void Awake()
        {
            img = GetComponent<Image>();
            unselectedColor = img.color;

            toggles = toggleParent.GetComponentsInChildren<Toggle>();
            foreach (var toggle in toggles) {
                if (toggle.gameObject != toggleParent.gameObject)
                    toggle.onValueChanged.AddListener(ChildToggled);
            }
        }

        private void ChildToggled(bool toggled)
        {
            if (toggled) {
                img.color = color;
                return;
            }

            foreach (var toggle in toggles) {
                if (toggle.gameObject == toggleParent.gameObject || !toggle.isOn)
                    continue;

                img.color = color;
                return;
            }

            img.color = unselectedColor;
        }
    }
}