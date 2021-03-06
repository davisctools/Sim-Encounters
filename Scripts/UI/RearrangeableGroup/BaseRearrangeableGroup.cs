﻿using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.UI
{
    public abstract class BaseRearrangeableGroup : MonoBehaviour
    {
        public abstract List<IDraggable> CurrentOrder { get; }
        public abstract event RearrangedEventHandler Rearranged;
        public abstract void Add(IDraggable draggable);
        public abstract void Remove(IDraggable draggable);
        public abstract void Clear();
    }
}