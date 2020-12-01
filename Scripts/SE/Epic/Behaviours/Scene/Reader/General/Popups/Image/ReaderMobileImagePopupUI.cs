using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileImagePopupUI : SpriteDrawer
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
        public RectTransform ImagePanel { get => imagePanel; set => imagePanel = value; }
        [SerializeField] private RectTransform imagePanel;

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public override void Display(Sprite sprite)
        {
            gameObject.SetActive(true);
            var heightPerWidth = ((float)sprite.texture.height) / sprite.texture.width;
            var panelWidth = ImagePanel.rect.width;
            var panelHeight = panelWidth * heightPerWidth;
            ImagePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);
            Image.sprite = sprite;
        }
    }
}