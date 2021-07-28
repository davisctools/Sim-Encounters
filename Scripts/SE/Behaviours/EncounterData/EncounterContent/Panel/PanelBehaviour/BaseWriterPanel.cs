using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterPanel : MonoBehaviour, ISelectedListener<PanelSelectedEventArgs>
    {
        protected abstract BaseWriterPanelsDrawer ChildPanelCreator { get; }
        protected abstract BaseWriterPinsDrawer PinsDrawer { get; }
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        protected Panel CurrentPanel => CurrentValue?.Panel;
        public PanelSelectedEventArgs CurrentValue { get; protected set; }

        public virtual event SelectedHandler<PanelSelectedEventArgs> Selected;

        protected IWriterPanelField[] Fields { get; set; }
        public virtual void Display(object sender, PanelSelectedEventArgs eventArgs)
        {
            if (CurrentValue == eventArgs)
                return;

            if (eventArgs.Panel == null)
                eventArgs = new PanelSelectedEventArgs(new Panel(type));

            CurrentValue = eventArgs;
            Fields = GetComponentsInChildren<IWriterPanelField>(true);
            Selected?.Invoke(sender, eventArgs);


            DrawChildPanels(eventArgs);
            DrawPins(eventArgs);
        }

        protected virtual void DrawChildPanels(PanelSelectedEventArgs eventArgs)
        {
            if (ChildPanelCreator != null)
                ChildPanelCreator.DrawChildPanels(eventArgs.Panel.ChildPanels);
        }
        protected virtual void DrawPins(PanelSelectedEventArgs eventArgs)
        {
            if (PinsDrawer == null)
                return;

            var pins = eventArgs.Panel.Pins;
            if (pins == null)
                pins = new PinGroup();
            PinsDrawer.Display(pins);
        }

        public virtual Panel Serialize()
        {
            var panel = new Panel(Type);
            if (PinsDrawer != null)
                panel.Pins = PinsDrawer.Serialize();
            if (ChildPanelCreator != null)
                panel.ChildPanels = ChildPanelCreator.SerializeChildren();
            panel.Values = SerializeFields(Fields);

            return panel;
        }

        protected virtual Dictionary<string, string> SerializeFields(IEnumerable<IWriterPanelField> panelFields)
        {
            var values = new Dictionary<string, string>();
            foreach (var panelField in panelFields) {
                if (string.IsNullOrWhiteSpace(panelField.Value))
                    continue;
                else if (values.ContainsKey(panelField.Name))
                    Debug.LogError($"Duplicate panel value field name ({panelField.Name})");
                else
                    values.Add(panelField.Name, panelField.Value);
            }

            return values;
        }


        public class Factory : PlaceholderFactory<UnityEngine.Object, BaseWriterPanel> { }
    }
}