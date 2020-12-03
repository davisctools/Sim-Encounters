using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class WriterColoredPanelImage : MonoBehaviour, IWriterPanelField
    {
        public virtual string Name => name;
        public virtual string Value => value;
        private string value;

        protected Image Image {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }
        private Image image;

        protected IStringDeserializer<Color> ColorDeserializer { get; set; }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(IStringDeserializer<Color> colorStringDeserializer, ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            ColorDeserializer = colorStringDeserializer;
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(this, PanelSelectedListener.CurrentValue);
        }


        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            if (!values.ContainsKey(Name))
                return;
            value = values[Name];
            Image.color = ColorDeserializer.Deserialize(value);
        }
    }
}