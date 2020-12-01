using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMProLinks : MonoBehaviour, IPointerClickHandler
    {
        TextMeshProUGUI text;

        // Start is called before the first frame update
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
            var linkURL = link.GetLinkID();

            Application.OpenURL(linkURL);
        }
    }
}