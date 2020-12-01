using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCategorySelector : MonoBehaviour,
        ISelectedListener<CategorySelectedEventArgs>
    {
        public virtual CategorySelectedEventArgs CurrentValue { get; protected set; }

        public event SelectedHandler<CategorySelectedEventArgs> Selected;
        protected virtual void Select(object sender, CategorySelectedEventArgs eventArgs)
        {
            CurrentValue = eventArgs;
            Selected?.Invoke(sender, eventArgs);
        }

        public abstract void Display(MenuSceneInfo sceneInfo, IEnumerable<Category> categories);
        public abstract void Show();
        public abstract void Hide();
    }
}