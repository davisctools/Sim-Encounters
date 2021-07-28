using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMProLinks : MonoBehaviour, IPointerClickHandler
    {
        TextMeshProUGUI text;

        private void Start() => text = GetComponent<TextMeshProUGUI>();

        public void OnPointerClick(PointerEventData eventData)
        {
            var linkCount = text.textInfo.linkInfo.Length;
            if (linkCount == 0)
                return;

            var linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, Camera.current);
            if (linkIndex < 0 || linkIndex >= linkCount)
                return;

            var link = text.textInfo.linkInfo[linkIndex];
            var linkUrl = link.GetLinkID();
            if (linkUrl.StartsWith("URL:", StringComparison.InvariantCultureIgnoreCase))
                linkUrl = linkUrl.Substring(4);

            Application.OpenURL(linkUrl);
        }
    }
}