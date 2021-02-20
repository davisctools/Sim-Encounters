using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class ResizableText : MonoBehaviour
    {
        private float defaultFontSize;

        protected TMP_Text Text { get; set; }
        [Inject]
        public virtual void Inject(CanvasResizer resizer)
        {
            Text = GetComponent<TMP_Text>();
            defaultFontSize = Text.fontSize;
            resizer.Resized += Resized;
            Resized(resizer.ResizeValue);
        }

        private void Resized(float size) => Text.fontSize = defaultFontSize * size;
    }
}