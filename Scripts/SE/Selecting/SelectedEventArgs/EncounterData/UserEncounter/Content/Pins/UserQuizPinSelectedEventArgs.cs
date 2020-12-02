using System;

namespace ClinicalTools.SimEncounters
{
    public class UserQuizPinSelectedEventArgs : EventArgs
    {
        public UserQuizPin Pin { get; }
        public UserQuizPinSelectedEventArgs(UserQuizPin pin) => Pin = pin;
    }
}