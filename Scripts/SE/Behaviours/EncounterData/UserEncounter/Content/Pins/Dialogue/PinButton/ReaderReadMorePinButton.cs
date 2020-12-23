using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderReadMorePinButton : BaseUserReadMorePinDrawer
    {
        public virtual Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;
        public virtual GameObject Completed { get => completed; set => completed = value; }
        [SerializeField] private GameObject completed;

        protected BaseUserReadMorePinDrawer ReadMorePopup { get; set; }
        [Inject] public virtual void Inject(BaseUserReadMorePinDrawer readMorePopup) => ReadMorePopup = readMorePopup;

        protected UserReadMorePin UserPin { get; set; }
        public override void Display(UserReadMorePin userPin)
        {
            if (UserPin != null)
                UserPin.StatusChanged -= OnStatusChanged;

            UserPin = userPin;
            UserPin.StatusChanged += OnStatusChanged;
            OnStatusChanged();

            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => ReadMorePopup.Display(userPin));
        }

        protected virtual void OnStatusChanged()
        {
            if (Completed != null)
                Completed.SetActive(UserPin.IsRead());
        }

        protected virtual void OnDestroy()
        {
            if (UserPin != null)
                UserPin.StatusChanged -= OnStatusChanged;
        }
    }
}