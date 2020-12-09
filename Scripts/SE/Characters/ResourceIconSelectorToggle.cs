using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ResourceIconSelectorToggle : IconSelectorToggle
    {
        public ColorType ColorType { get => colorType; set => colorType = value; }
        [SerializeField] private ColorType colorType;

        protected IColorManager ColorManager { get; set; }
        [Inject] public virtual void Inject(IColorManager colorManager) => ColorManager = colorManager;

        [SerializeField] private string reference;

        protected override void Start()
        {
            base.Start();
            Icon = new Icon(Icon.IconType.Resource, reference, ColorManager.GetColor(ColorType));
        }
    }
}