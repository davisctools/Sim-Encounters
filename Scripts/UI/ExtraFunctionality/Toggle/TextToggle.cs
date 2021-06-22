using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public abstract class TextToggle : MonoBehaviour
    {
        public abstract Toggle Toggle { get; set; }
        public abstract string Text { get; set; }
    }
}