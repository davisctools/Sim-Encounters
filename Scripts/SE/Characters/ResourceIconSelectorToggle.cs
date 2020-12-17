using ClinicalTools.UI;
using System.IO;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ResourceIconSelectorToggle : IconSelectorToggle
    {
        [SerializeField] private string resourceFolder = "Characters";

        protected IColorManager ColorManager { get; set; }
        [Inject] public virtual void Inject(IColorManager colorManager) => ColorManager = colorManager;

        public override Icon Icon {
            get {
                if (base.Icon == null)
                    base.Icon = new Icon(Icon.IconType.Resource, Path.Combine(resourceFolder, IconDisplay.sprite.name));
                return base.Icon;
            }
            protected set => base.Icon = value;
        }
    }
}