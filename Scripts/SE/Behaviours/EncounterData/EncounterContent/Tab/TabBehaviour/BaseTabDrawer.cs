﻿using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseTabDrawer : MonoBehaviour, ISelectedListener<TabSelectedEventArgs>
    {
        public TabSelectedEventArgs CurrentValue { get; protected set;}
        public event SelectedHandler<TabSelectedEventArgs> Selected;

        public virtual void Display(object sender, TabSelectedEventArgs eventArgs)
        {
            CurrentValue = eventArgs;
            Selected?.Invoke(sender, eventArgs);
        }

        public abstract Tab Serialize();

        public class Factory : PlaceholderFactory<string, BaseTabDrawer> { }
    }
}