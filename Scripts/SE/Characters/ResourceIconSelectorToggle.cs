using ClinicalTools.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ResourceIconSelectorToggle : IconSelectorToggle
    {
        protected IColorManager ColorManager { get; set; }
        [Inject] public virtual void Inject(IColorManager colorManager) => ColorManager = colorManager;

        public override Icon Icon {
            get {
                if (base.Icon == null)
                    base.Icon = new Icon(Icon.IconType.Resource, IconDisplay.sprite.name);
                return base.Icon;
            }
            protected set => base.Icon = value;
        }
    }
}