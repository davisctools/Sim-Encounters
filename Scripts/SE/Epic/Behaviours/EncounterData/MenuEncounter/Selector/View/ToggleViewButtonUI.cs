using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ToggleViewButtonUI : MonoBehaviour
    {
        public event Action Selected;

        public TextMeshProUGUI Text { get => text; set => text = value; }
        [SerializeField] private TextMeshProUGUI text;
        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;

        protected virtual void Awake()
        {
            Button.onClick.AddListener(() => Selected?.Invoke());
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            Button.interactable = true;
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            Button.interactable = false;
        }

        public virtual void Display(BaseViewEncounterSelector encountersViewUI)
        {
            Text.text = encountersViewUI.ViewName;
            Image.sprite = encountersViewUI.ViewSprite;
        }
    }
}