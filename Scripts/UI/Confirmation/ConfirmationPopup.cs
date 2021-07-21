using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.UI
{
    public class ConfirmationPopup : BaseConfirmationPopup, ICloseHandler
    {
        public TextMeshProUGUI Title { get => title; set => title = value; }
        [SerializeField] private TextMeshProUGUI title;
        public TextMeshProUGUI Description { get => description; set => description = value; }
        [SerializeField] private TextMeshProUGUI description;
        public Button ConfirmationButton { get => confirmationButton; set => confirmationButton = value; }
        [SerializeField] private Button confirmationButton;
        public List<Button> CancellationButtons { get => cancellationButtons; set => cancellationButtons = value; }
        [SerializeField] private List<Button> cancellationButtons;
        public TextMeshProUGUI ConfirmationLabel { get => confirmationLabel; set => confirmationLabel = value; }
        [SerializeField] private TextMeshProUGUI confirmationLabel;
        public TextMeshProUGUI CancellationLabel { get => cancellationLabel; set => cancellationLabel = value; }
        [SerializeField] private TextMeshProUGUI cancellationLabel;

#if MOBILE
        protected AndroidBackButton BackButton { get; set; }
        [Inject]
        public virtual void Inject(AndroidBackButton backButton) => BackButton = backButton;
#endif

        protected Action ConfirmationAction { get; set; }
        protected Action CancellationAction { get; set; }


        protected virtual void Awake()
        {
            foreach (var cancellationButton in CancellationButtons)
                cancellationButton.onClick.AddListener(Cancel);
            ConfirmationButton.onClick.AddListener(Confirm);
        }

        public override void ShowConfirmation(Action confirmationAction, string title, string description,
            string confirmationText = "Yes", string cancellationText = "Cancel")
            => ShowConfirmation(confirmationAction, null, title, description, confirmationText, cancellationText);
        public override void ShowConfirmation(Action confirmationAction, Action cancellationAction, string title, string description,
            string confirmationText = "Yes", string cancellationText = "Cancel")
        {
            ConfirmationAction = confirmationAction;
            CancellationAction = cancellationAction;
            Title.text = title;
            if (Description != null) {
                Description.gameObject.SetActive(!string.IsNullOrWhiteSpace(description));
                Description.text = description;
            }
            gameObject.SetActive(true);
            if (ConfirmationLabel != null)
                ConfirmationLabel.text = confirmationText.ToUpper();
            if (CancellationLabel != null)
                CancellationLabel.text = cancellationText.ToUpper();
#if MOBILE
            BackButton.Register(Cancel);
#endif
        }

        protected virtual void Confirm()
        {
            ConfirmationAction?.Invoke();
            Close();
        }

        protected virtual void Cancel()
        {
            CancellationAction?.Invoke();
            Close();
        }

        protected virtual void Close()
        {
            ConfirmationAction = null;
            CancellationAction = null;
            gameObject.SetActive(false);
#if MOBILE
            BackButton.Deregister(Cancel);
#endif
        }

        public void Close(object sender) => Cancel();
    }
}