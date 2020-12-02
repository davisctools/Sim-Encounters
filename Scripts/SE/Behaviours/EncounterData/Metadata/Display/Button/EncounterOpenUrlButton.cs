using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class EncounterOpenUrlButton : EncounterMetadataButton
    {
        protected override void Start()
        {
            base.Start();
            Button.onClick.AddListener(OpenUrl);
        }

        protected string Url { get; set; }
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
        {
            if (eventArgs.Metadata is IWebCompletion webCompletion)
                Url = webCompletion.Url;
        }

        protected virtual void OpenUrl()
        {
            if (!string.IsNullOrWhiteSpace(Url))
                Application.OpenURL(Url);
        }
    }
}