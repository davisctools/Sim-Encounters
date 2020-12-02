using ClinicalTools.UI;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterCompletionCodeCopyButton : EncounterMetadataButton
    {
        protected BaseTooltip Tooltip { get; set; }
        [Inject] protected virtual void Inject(BaseTooltip tooltip) => Tooltip = tooltip;

        protected override void Start()
        {
            base.Start();
            Button.onClick.AddListener(CopyCode);
        }

        protected string CompletionCode { get; set; }
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
        {
            if (eventArgs.Metadata is IWebCompletion webCompletion)
                CompletionCode = webCompletion.CompletionCode;
        }

        protected virtual void CopyCode()
        {
            if (!string.IsNullOrWhiteSpace(CompletionCode))
                Tooltip.Show();
        }
    }
}