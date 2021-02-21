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
        protected CanvasResizer Resizer { get; set; }
        [Inject]
        public virtual void Inject(CanvasResizer resizer)
        {
            Text = GetComponent<TMP_Text>();
            defaultFontSize = Text.fontSize;

            Resizer = resizer;
            Resizer.Resized += Resized;
            Resized(Resizer.ResizeValue);
        }

        private void Resized(float size) => Text.fontSize = defaultFontSize * size;

        protected virtual void OnDestroy()
        {
            if (Resizer) Resizer.Resized -= Resized;
        }
    }
}