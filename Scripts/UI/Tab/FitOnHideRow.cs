using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class FitOnHideRow : TabRow
    {
        private ContentSizeFitter fitter;
        protected override void Start()
        {
            fitter = GetComponent<ContentSizeFitter>();
            base.Start();
            Update();
        }

        protected virtual void Update() => fitter.enabled = !Visible;
    }
}