using System;

namespace ClinicalTools.SimEncounters
{
    public class UserDialoguePinSelectedEventArgs : EventArgs
    {
        public UserDialoguePin Pin { get; }
        public UserDialoguePinSelectedEventArgs(UserDialoguePin pin) => Pin = pin;
    }
}