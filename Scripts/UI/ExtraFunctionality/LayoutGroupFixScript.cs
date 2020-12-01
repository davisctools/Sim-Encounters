using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class LayoutGroupFixScript : MonoBehaviour
    {
        protected VerticalLayoutGroup VerticalLayoutGroup { get; set; }
        protected HorizontalLayoutGroup HorizontalLayoutGroup { get; set; }
        void Awake()
        {
            Debug.LogWarning("LayoutGroupFixScript");
            VerticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            if (VerticalLayoutGroup)
                VerticalLayoutGroup.enabled = false;

            HorizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
            if (HorizontalLayoutGroup)
                HorizontalLayoutGroup.enabled = false;
        }


        // Use this for initialization
        void Start() => NextFrame.Function(Fix);

        private void Fix()
        {
            if (VerticalLayoutGroup)
                VerticalLayoutGroup.enabled = true;

            if (HorizontalLayoutGroup)
                HorizontalLayoutGroup.enabled = true;
        }
    }
}