using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class TabScrollScript : MonoBehaviour
    {
        public Scrollbar Scroll { get => scroll; set => scroll = value; }
        [SerializeField] private Scrollbar scroll;
        public TabScrollButtonScript LeftButton { get => leftButton; set => leftButton = value; }
        [SerializeField] private TabScrollButtonScript leftButton;
        public TabScrollButtonScript RightButton { get => rightButton; set => rightButton = value; }
        [SerializeField] private TabScrollButtonScript rightButton;

        private const float SPEED = 1;
        private bool showingButtons = true;

        protected virtual void Update()
        {
            if (Scroll.size > .99f) {
                if (showingButtons)
                    HideButtons();
                return;
            }

            showingButtons = true;
            if (LeftButton.IsDown)
                Scroll.value -= GetDistance();
            else if (RightButton.IsDown)
                Scroll.value += GetDistance();

            LeftButton.SetActive(Scroll.value > .01f);
            RightButton.SetActive(Scroll.value < .99f);
        }

        protected virtual float GetDistance() => Time.deltaTime * SPEED / (1 - Scroll.size);
        protected virtual void HideButtons()
        {
            showingButtons = false;
            LeftButton.SetActive(false);
            RightButton.SetActive(false);
        }
    }
}