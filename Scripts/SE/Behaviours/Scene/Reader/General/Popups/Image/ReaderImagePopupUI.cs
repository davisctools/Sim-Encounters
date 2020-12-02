using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderImagePopupUI : SpriteDrawer
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();

        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public override void Display(Sprite sprite)
        {
            gameObject.SetActive(true);
            Image.sprite = sprite;
        }
    }
}