﻿using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseIconSelector : MonoBehaviour
    {
        public abstract event Action<Icon> ValueChanged;

        public abstract void Display(Icon icon);
        public abstract Icon GetValue();
    }
}