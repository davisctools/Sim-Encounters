using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Toggle))]
    public class StringToggle : TextToggle
    {
        public override Toggle Toggle { get ; set ; }

        public override string Text { get => text; set => text = value; }
        [SerializeField] private string text;

        protected virtual void Awake() => Toggle = GetComponent<Toggle>();
    }
}