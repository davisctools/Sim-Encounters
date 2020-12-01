using System;
using System.Collections.Generic;

namespace ClinicalTools.UI
{
    public delegate void RearrangedEventHandler(object sender, RearrangedEventArgs2 e);
    public class RearrangedEventArgs2 : EventArgs
    {
        public int OldIndex { get; }
        public int NewIndex { get; }
        public IDraggable MovedObject { get; }
        public List<IDraggable> CurrentOrder { get; }

        public RearrangedEventArgs2(int oldIndex, int newIndex, IDraggable movedObject, List<IDraggable> draggedObjects)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            MovedObject = movedObject;
            CurrentOrder = draggedObjects;
        }
    }
}