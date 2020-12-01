using System;
using UnityEngine;

namespace ClinicalTools.UI
{
    public abstract class BaseConfirmationPopup : MonoBehaviour
    {
        public abstract void ShowConfirmation(Action confirmationAction, string title, string description,
            string confirmationText = "Yes", string cancellationText = "Cancel");
        public abstract void ShowConfirmation(Action confirmationAction, Action cancellationAction, string title, string description,
            string confirmationText = "Yes", string cancellationText = "Cancel");
    }
}