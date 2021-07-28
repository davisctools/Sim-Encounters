using System;

namespace ClinicalTools.SimEncounters
{
    public class Selector<T> : ISelector<T>, ISelectedListener<T>
        where T : EventArgs
    {
        public event SelectedHandler<T> Selected;

        public object CurrentSender { get; protected set; }
        public T CurrentValue { get; protected set; }

        public virtual void Select(object sender, T value)
        {
            if (CurrentSender == sender && CurrentValue.Equals(value))
                return;
            CurrentSender = sender;
            CurrentValue = value;
            Selected?.Invoke(sender, value);
        }
    }
}