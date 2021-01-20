using System;

namespace ClinicalTools.SimEncounters
{
    public class CategorySelectedEventArgs : EventArgs
    {
        public Category Category { get; }
        public CategorySelectedEventArgs(Category category)
        {
            Category = category;
        }
    }
}