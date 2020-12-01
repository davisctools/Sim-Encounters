using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class SwitchImageToggle : MonoBehaviour
    {
        protected Sprite OnSprite { get => onSprite; set => onSprite = value; }
        [SerializeField] private Sprite onSprite;
        protected Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;

        protected Sprite OffSprite { get; set; }
        protected virtual void Awake()
        {
            OffSprite = Toggle.image.sprite;
            if (Toggle.isOn)
                Toggle.image.sprite = OnSprite;
            Toggle.onValueChanged.AddListener(ToggleChanged);
        }

        protected virtual void ToggleChanged(bool value)
            => Toggle.image.sprite = value ? OnSprite : OffSprite;
    }
}
