using ClinicalTools.SEColors;
using System.IO;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ResourceIconSelectorToggle : IconSelectorToggle
    {
        [SerializeField] private string resourceFolder = "Characters";

        protected virtual IColorManager ColorManager { get; } = new ColorManager();

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