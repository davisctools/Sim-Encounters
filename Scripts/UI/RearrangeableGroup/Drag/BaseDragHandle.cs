using System;
using UnityEngine;

namespace ClinicalTools.UI
{
    public abstract class BaseDragHandle : MonoBehaviour
    {
        public abstract event Action StartDragging;
    }
}