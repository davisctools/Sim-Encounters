using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterReadMorePopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button RemoveButton { get => removeButton; set => removeButton = value; }
        [SerializeField] private Button removeButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;

        public BaseWriterPanelsDrawer PanelsDrawer { get => panelsDrawer; set => panelsDrawer = value; }
        [SerializeField] private BaseWriterPanelsDrawer panelsDrawer;

        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            RemoveButton.onClick.AddListener(ConfirmRemove);
            ApplyButton.onClick.AddListener(Apply);
        }

        protected WaitableTask<ReadMorePin> CurrentWaitablePin { get; set; }
        protected ReadMorePin CurrentPin { get; set; }
        public virtual WaitableTask<ReadMorePin> EditReadMore(ReadMorePin pin)
        {
            CurrentPin = pin;

            if (CurrentWaitablePin?.IsCompleted() == false)
                CurrentWaitablePin.SetError(new Exception("New popup opened"));

            CurrentWaitablePin = new WaitableTask<ReadMorePin>();

            gameObject.SetActive(true);

            if (pin.Panels.Count == 0)
                PanelsDrawer.DrawDefaultChildPanels();
            else
                PanelsDrawer.DrawChildPanels(pin.Panels);

            return CurrentWaitablePin;
        }
        protected virtual void Apply()
        {
            CurrentPin.Panels = PanelsDrawer.SerializeChildren();
            CurrentWaitablePin.SetResult(CurrentPin);

            Close();
        }
        protected virtual void ConfirmRemove() => ConfirmationPopup.ShowConfirmation(Remove, "Confirm", "Are you sure you want to remove this dialogue?");
        protected virtual void Remove()
        {
            CurrentWaitablePin.SetResult(null);
            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitablePin?.IsCompleted() == false)
                CurrentWaitablePin.SetError(new Exception("Canceled"));

            gameObject.SetActive(false);
        }
    }
}