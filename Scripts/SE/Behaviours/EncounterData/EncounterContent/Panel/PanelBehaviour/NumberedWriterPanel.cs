using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class NumberedWriterPanel : WriterAddablePanel
    {
        public string NumberPrefix { get => numberPrefix; set => numberPrefix = value; }
        [SerializeField] private string numberPrefix;
        public TextMeshProUGUI NumberLabel { get => numberLabel; set => numberLabel = value; }
        [SerializeField] private TextMeshProUGUI numberLabel;

        protected int SiblingIndex { get; set; } = -1;
        protected override void Awake()
        {
            UpdateNumberLabel();
            base.Awake();
        }
        protected virtual void Update() => UpdateNumberLabel();

        protected virtual void UpdateNumberLabel()
        {
            var newSiblingIndex = gameObject.transform.GetSiblingIndex();
            if (newSiblingIndex == SiblingIndex)
                return;

            SiblingIndex = newSiblingIndex;
            NumberLabel.text = $"{NumberPrefix}{SiblingIndex + 1}";
        }
    }
}