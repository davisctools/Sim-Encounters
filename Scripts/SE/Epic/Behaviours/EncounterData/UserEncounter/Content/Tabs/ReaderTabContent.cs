using System.Diagnostics;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(RectTransform))]
    public class ReaderTabContent : UserTabSelectorBehaviour
    {
        public RectTransform RectTransform => (RectTransform)transform;
        public UserTab Tab => UserTabValue?.SelectedTab;

        public override void Select(object sender, UserTabSelectedEventArgs eventArgs)
        {
            var stopwatch = Stopwatch.StartNew();
            base.Select(sender, eventArgs);
            UnityEngine.Debug.LogWarning($"D. {eventArgs.SelectedTab.Data.Name}: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
